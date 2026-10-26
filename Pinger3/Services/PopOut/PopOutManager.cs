using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;
using Pinger3.Services.Pinging;

namespace Pinger3.Services.PopOut;

public sealed class PopOutManager : IPopOutManager, IDisposable
{
    private readonly IEndpointRepository _repository;
    private readonly IPopOutEndpointsRegistry _registry;
    private readonly List<IDisposable> _subscriptions;

    public PopOutManager(IPingManager manager, IEndpointRepository repository, IPopOutEndpointsRegistry registry,
        IEndpointUpdateNotifier updateNotifier)
    {
        _repository = repository;
        _registry = registry;
        _subscriptions =
        [
            updateNotifier.SubscribeToUpdate(async (model, _) =>  await OnEndpointUpdated(model)),
            updateNotifier.SubscribeToDeletion(async (id, ct) => await OnEndpointRemoved(id, ct))
        ];
        manager.PingReceived += (sender, updated) =>
        {
            if (IsPopOut(updated.Id))
                PingReceived?.Invoke(sender, updated);
        };
        manager.PingSent += (sender, e) =>
        {
            if (IsPopOut(e))
                PingSent?.Invoke(sender, e);
        };
    }

    private ValueTask OnEndpointUpdated(EndpointModel endpoint)
    {
        EndpointUpdated?.Invoke(this, endpoint);
        return ValueTask.CompletedTask;
    }

    public event EventHandler<EndpointModel>? EndpointAdded;
    public event EventHandler<string>? EndpointRemoved;

    public event EventHandler<EndpointModel>? EndpointUpdated;
    public event EventHandler<PingUpdated>? PingReceived;
    public event EventHandler<string>? PingSent;

    public async Task Add(string id, CancellationToken ct)
    {
        if (_registry.IsPopOut(id))
            return;
        var endpoint = _repository.GetEndpoint(id);

        if (endpoint == null)
            return;

        await _registry.AddPopOutAsync(id, ct);
        await OnEndpointAdded(endpoint, ct);
    }

    public async Task Toggle(string id, CancellationToken ct)
    {
        if (IsPopOut(id))
            await Remove(id, ct);
        else
            await Add(id, ct);
    }

    public async Task Remove(string id, CancellationToken ct)
    {
        await _registry.RemovePopOutAsync(id, ct);
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