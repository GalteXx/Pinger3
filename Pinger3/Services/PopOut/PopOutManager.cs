using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.PopOut;

public sealed class PopOutManager : IPopOutManager, IDisposable
{
    private readonly IEndpointRepository _repository;
    private readonly IPopOutEndpointsRegistry _registry;
    private readonly List<IDisposable> _subscriptions;

    public PopOutManager(IEndpointRepository repository, IPopOutEndpointsRegistry registry,
        IEndpointUpdateNotifier updateNotifier)
    {
        _repository = repository;
        _registry = registry;
        _subscriptions =
        [
            updateNotifier.SubscribeToDeletion(async (id, token) => await registry.RemovePopOutAsync(id, token))
        ];
    }

    public event EventHandler<EndpointModel>? EndpointAdded;
    public event EventHandler<string>? EndpointRemoved;

    public async Task Add(string id, CancellationToken ct)
    {
        if (_registry.IsPopOut(id))
            return;
        var endpoint = _repository.GetEndpoint(id);

        if (endpoint == null)
            return;
        await OnEndpointAdded(endpoint, ct);
    }

    public async Task Toggle(string id, CancellationToken ct)
    {
        if (IsPopOut(id))
            await Remove(id, ct);
        else
            await Add(id, ct);
    }

    public IEnumerable<EndpointModel> GetEndpointModels() =>
        _registry.GetPopOutIds().Select(id => _repository.GetEndpoint(id)).OfType<EndpointModel>();

    public async Task Remove(string id, CancellationToken ct)
    {
        await OnEndpointRemoved(id, ct);
    }

    public bool IsPopOut(string id)
    {
        return _registry.IsPopOut(id);
    }

    private async Task OnEndpointAdded(EndpointModel model, CancellationToken ct)
    {
        await _registry.AddPopOutAsync(model.Id, ct);
        EndpointAdded?.Invoke(this, model);
    }

    private async Task OnEndpointRemoved(string id, CancellationToken ct)
    {
        await _registry.RemovePopOutAsync(id, ct);
        EndpointRemoved?.Invoke(this, id);
    }

    public void Dispose()
    {
        foreach (var subscription in _subscriptions)
            subscription.Dispose();
    }
}