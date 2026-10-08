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

    public PingScheduler(IPingTransport transport)
    {
        _transport = transport;
        var pingingCycleTask = RunAsync(CancellationToken.None);
        pingingCycleTask.ContinueWith(t =>
        {
            if (t.IsFaulted)
                throw new Exception($"Unhandled exception in PingScheduler: {t.Exception?.Flatten()}");
        }, TaskScheduler.Default);
    }

    public Dictionary<string, EndpointRuntime> Endpoints { get; } = [];
    public event EventHandler<string>? PingSent;
    public event EventHandler<PingUpdated>? PingReceived;

    private async Task RunAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var due = Endpoints.Values
                .Where(e => e.LastPinged + e.DelayBetweenRequests <= now && !e.IsPinged);

            foreach (var runtime in due)
            {
                _ = SendPingAsync(runtime, ct);
            }

            await Task.Delay(_tick, ct);
        }
    }

    private async Task SendPingAsync(EndpointRuntime runtime, CancellationToken token)
    {
        runtime.IsPinged = true;
        
        PingSent?.Invoke(this, runtime.Id);
        
        var result = await _transport.PingAsync(runtime, token);
        runtime.Ping = result;

        runtime.LastPinged = DateTime.Now;
        runtime.IsPinged = false;
        PingReceived?.Invoke(this, new PingUpdated(runtime.Id, runtime.Ping));
    }
}