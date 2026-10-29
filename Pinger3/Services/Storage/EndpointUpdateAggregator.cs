using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.DataTypes;
using Pinger3.Models;

namespace Pinger3.Services;

public class EndpointUpdateAggregator : IEndpointUpdateNotifier, IEndpointUpdatePublisher
{
    private readonly AsyncEvent<EndpointModel> _update = new();
    private readonly AsyncEvent<string> _deletion = new();

    public IDisposable SubscribeToUpdate(Func<EndpointModel, CancellationToken, ValueTask> handler)
        => _update.Subscribe(handler);

    public IDisposable SubscribeToDeletion(Func<string, CancellationToken, ValueTask> handler)
        => _deletion.Subscribe(handler);

    public ValueTask PublishUpdateAsync(EndpointModel updatedModel, CancellationToken ct = default)
        => _update.PublishAsync(updatedModel, ct);

    public ValueTask PublishDeletionAsync(string modelId, CancellationToken ct = default)
        => _deletion.PublishAsync(modelId, ct);
}