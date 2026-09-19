using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

internal sealed class PingScheduler(IPingTransport transport, EndpointRuntimeFactory factory)
{
    private readonly List<EndpointRuntime> _endpoints = [];
    private readonly TimeSpan _tick = TimeSpan.FromMilliseconds(100);
    private const int MaxConcurrency = 10;

    public event EventHandler<EndpointUpdated>? PingCompleted;

    private async Task RunAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var due = _endpoints
                .Where(e => e.LastPinged + e.Model.DelayBetweenRequests <= now);

            await Parallel.ForEachAsync(due,
                new ParallelOptions { MaxDegreeOfParallelism = MaxConcurrency, CancellationToken = ct },
                async (runtime, token) =>
                {
                    runtime.LastPinged = DateTime.Now;

                    TimeSpan? rtt = null;

                    var result = await transport.PingAsync(runtime.Model, token);

                    runtime.Ping = result;

                    PingCompleted?.Invoke(this,
                        new EndpointUpdated(runtime.Model.Id, runtime.Ping, runtime.LastPinged));
                });

            await Task.Delay(_tick, ct);
        }
    }

    public void AddModel(EndpointModel model)
    {
        var runtime = factory.Create(model);
        _endpoints.Add(runtime);
    }

    public void RemoveModel(EndpointModel model)
    {
        _endpoints.RemoveAll(e => e.Model.Id == model.Id);
    }
}