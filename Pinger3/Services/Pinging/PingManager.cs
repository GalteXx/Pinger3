using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public sealed class PingManager : IPingManager, IDisposable
{
    private readonly IActiveEndpointsRegistry _activeEndpointsRegistry;
    private readonly IEndpointRuntimeFactory _factory;
    private readonly IEndpointRepository _repository;
    private readonly IPingScheduler _scheduler;

    private readonly List<IDisposable> _subscriptions;
    private readonly EndpointUpdateResolver _updateResolver;

    public PingManager(IPingScheduler scheduler, IEndpointRuntimeFactory factory, IEndpointRepository repository,
        EndpointUpdateResolver updateResolver, IEndpointUpdateNotifier updateNotifier,
        IActiveEndpointsRegistry activeEndpointsRegistry)
    {
        _scheduler = scheduler;
        _factory = factory;
        _repository = repository;
        _updateResolver = updateResolver;
        _activeEndpointsRegistry = activeEndpointsRegistry;
        _subscriptions =
        [
            updateNotifier.SubscribeToUpdate(async (model, ct) => await OnPingModelUpdated(model, ct)),
            updateNotifier.SubscribeToDeletion((id, _) => OnPingModelRemoved(id))
        ];
    }

    public void Dispose()
    {
        foreach (var subscription in _subscriptions)
            subscription.Dispose();
    }

    public async Task ToggleEndpointPingingAsync(string id, CancellationToken ct)
    {
        if (!IsRunning(id))
            await AddEndpointAsync(id, ct);
        else
            await RemoveEndpoint(id, ct);
    }

    public async Task AddEndpointAsync(string id, CancellationToken ct)
    {
        var model = _repository.GetEndpoint(id);
        if (model == null)
            return;
        var runtime = await _factory.CreateRuntimeAsync(model, ct);
        await _activeEndpointsRegistry.ActivateAsync(id, ct);
        _scheduler.AddToSchedule(runtime);
    }

    public async Task RemoveEndpoint(string id, CancellationToken ct)
    {
        await _activeEndpointsRegistry.DeactivateAsync(id, ct);
        _scheduler.RemoveFromSchedule(id);
    }

    public bool IsRunning(string id)
    {
        return _activeEndpointsRegistry.IsActive(id);
    }

    private async Task OnPingModelUpdated(EndpointModel model, CancellationToken ct)
    {
        if (_activeEndpointsRegistry.IsActive(model.Id))
            return;

        var update = await _updateResolver.ResolveAsync(model, CancellationToken.None);
        if (update is null)
            return;
        _scheduler.UpdateInSchedule(model.Id, update);
    }

    private ValueTask OnPingModelRemoved(string id)
    {
        if (!_activeEndpointsRegistry.IsActive(id))
            return ValueTask.CompletedTask;
        _scheduler.RemoveFromSchedule(id);
        return ValueTask.CompletedTask;
    }
}