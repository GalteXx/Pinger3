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
    private readonly Dictionary<string, EndpointRuntime> _endpoints = [];

    public PingScheduler(IPingTransport transport, IPingUpdatePublisher updatePublisher)
    {
        _transport = transport;
        var pingingCycleTask = RunAsync(CancellationToken.None);
        pingingCycleTask.ContinueWith(t =>
        {
            if (t.IsFaulted)
                throw new Exception($"Unhandled exception in PingScheduler: {t.Exception?.Flatten()}");
        }, TaskScheduler.Default);
    }

    public void AddToSchedule(EndpointRuntime endpoint) => _endpoints.Add(endpoint.Id, endpoint);

    public void RemoveFromSchedule(string endpointId) => _endpoints.Remove(endpointId);

    public void UpdateInSchedule(string id, ResolvedEndpointUpdate update)
    {
        _endpoints[id].UpdateAddress(update); // should throw
    }
    

    private async Task RunAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var due = _endpoints.Values
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