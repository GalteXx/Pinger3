using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public class EndpointUpdateAggregator : IEndpointUpdateNotifier, IEndpointUpdatePublisher
{
    public IDisposable SubscribeToDeletion(Func<string, CancellationToken, ValueTask> handler)
    {
        throw new NotImplementedException();
    }

    public IDisposable SubscribeToUpdate(Func<EndpointModel, CancellationToken, ValueTask> handler)
    {
        throw new NotImplementedException();
    }

    public ValueTask PublishUpdateAsync(EndpointModel updatedModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask PublishDeletionAsync(string modelId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}