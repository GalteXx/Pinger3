using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.DataTypes;
using Pinger3.Models;

namespace Pinger3.Services.PopOut;

public class PopOutEventAggregator : IPopOutEventPublisher, IPopOutEventNotifier
{
    private readonly AsyncEvent<EndpointModel> _endpointAddition = new();
    private readonly AsyncEvent<string> _endpointRemoval = new();

    public IDisposable SubscribeToRemoval(Func<string, CancellationToken, ValueTask> handler)
    {
        return _endpointRemoval.Subscribe(handler);
    }

    public IDisposable SubscribeToAddition(Func<EndpointModel, CancellationToken, ValueTask> handler)
    {
        return _endpointAddition.Subscribe(handler);
    }

    public async ValueTask PublishAdditionAsync(EndpointModel model, CancellationToken ct)
    {
        await _endpointAddition.PublishAsync(model, ct);
    }

    public async ValueTask PublishRemovalAsync(string id, CancellationToken ct)
    {
        await _endpointRemoval.PublishAsync(id, ct);
    }
}