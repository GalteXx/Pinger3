using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.DataTypes;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class PingUpdateAggregator
{
    private readonly AsyncEvent<string> _pingSent = new();
    private readonly AsyncEvent<PingUpdated> _pingUpdated = new();

    public IDisposable SubscribeToPingUpdated(Func<PingUpdated, CancellationToken, ValueTask> handler)
        => _pingUpdated.Subscribe(handler);

    public IDisposable SubscribeToPingSent(Func<string, CancellationToken, ValueTask> handler)
        => _pingSent.Subscribe(handler);

    public ValueTask PublishPingUpdateAsync(PingUpdated updatedModel, CancellationToken ct = default)
        => _pingUpdated.PublishAsync(updatedModel, ct);

    public ValueTask PublishPingSentAsync(string id, CancellationToken ct = default)
        => _pingSent.PublishAsync(id, ct);
}