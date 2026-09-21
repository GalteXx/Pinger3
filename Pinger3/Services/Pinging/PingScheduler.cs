using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public sealed class PingScheduler : IPingScheduler
{
    private readonly TimeSpan _tick = TimeSpan.FromMilliseconds(100);
    private readonly IPingTransport _transport;
    private const int MaxConcurrency = 10;

    public PingScheduler(IPingTransport transport, EndpointRuntimeFactory factory)
    {
        _transport = transport;
        RunAsync(CancellationToken.None).Start();
    }

    public Dictionary<string, EndpointRuntime> Endpoints { get; } = [];
    public event EventHandler<string>? PingSent;
    public event EventHandler<PingUpdated>? PingReceived;

    private async Task RunAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var due = Endpoints.Values
                .Where(e => e.LastPinged + e.DelayBetweenRequests <= now);

            await Parallel.ForEachAsync(due,
                new ParallelOptions { MaxDegreeOfParallelism = MaxConcurrency, CancellationToken = ct },
                async (runtime, token) =>
                {
                    runtime.LastPinged = DateTime.Now;
                    PingSent?.Invoke(this, runtime.Id);

                    var result = await _transport.PingAsync(runtime, token);
                    runtime.Ping = result;

                    PingReceived?.Invoke(this, new PingUpdated("Id", runtime.Ping));
                });

            await Task.Delay(_tick, ct);
        }
    }
}