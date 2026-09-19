using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public sealed class PingScheduler : IAsyncDisposable
{
    private readonly List<EndpointRuntime> _endpoints;
    private readonly IPingTransport _transport;
    private readonly TimeSpan _tick = TimeSpan.FromMilliseconds(100);
    private readonly int _maxConcurrency;

    public PingScheduler()
    {
        _endpoints = [];
    }

    public event EventHandler<EndpointUpdated>? PingCompleted;

    private async Task RunAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var due = _endpoints
                .Where(e => e.LastPinged + e.Model.DelayBetweenRequests <= now);

            await Parallel.ForEachAsync(due,
                new ParallelOptions { MaxDegreeOfParallelism = _maxConcurrency, CancellationToken = ct },
                async (runtime, token) =>
                {
                    runtime.LastPinged = DateTime.Now;

                    TimeSpan? rtt = null;
                    try
                    {
                        var result = await _transport.PingAsync(runtime.Model, token);
                        if (result.Success) rtt = result.Rtt;
                    }
                    catch (OperationCanceledException) { throw; }
                    catch { /* rtt stays null */ }

                    var finished = _clock.GetUtcNow().UtcDateTime;
                    runtime.MarkReply(finished, rtt);

                    // Schedule next due with optional backoff on failure
                    var delay = runtime.Model.DelayBetweenRequests;
                    if (!runtime.LastSucceeded)
                        delay = TimeSpan.FromTicks(Math.Min(
                            delay.Ticks * (1L << Math.Min(runtime.ConsecutiveFailures, 5)),
                            TimeSpan.FromMinutes(5).Ticks));

                    runtime.NextDueUtc = finished + delay;

                    PingCompleted?.Invoke(this,
                        new PingCompletedEventArgs(runtime.Model, rtt));
                });

            await Task.Delay(_tick, _clock, ct);
        }
    }
}