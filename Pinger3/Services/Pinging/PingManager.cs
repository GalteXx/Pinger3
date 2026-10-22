using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public sealed class PingManager : IPingManager, IDisposable
{
    private readonly IPingScheduler _scheduler;
    private readonly EndpointRuntimeFactory _factory;
    private readonly IEndpointRepository _repository;
    private readonly EndpointUpdateResolver _updateResolver;
    private readonly IActiveEndpointsStorage _activeEndpointsStorage;
    
    private readonly List<IDisposable> _subscriptions;

    public PingManager(IPingScheduler scheduler, EndpointRuntimeFactory factory, IEndpointRepository repository,
        EndpointUpdateResolver updateResolver, IActiveEndpointsStorage activeEndpointsStorage, IEndpointUpdateNotifier updateNotifier)
    {
        _scheduler = scheduler;
        _factory = factory;
        _repository = repository;
        _updateResolver = updateResolver;
        _activeEndpointsStorage = activeEndpointsStorage;
        _subscriptions =
        [
            updateNotifier.SubscribeToUpdate(async (model, ct) => await OnPingModelUpdated(model, ct))
        ];

        scheduler.PingSent += (sender, e) => PingSent?.Invoke(sender, e);
        scheduler.PingReceived += (sender, e) => PingReceived?.Invoke(sender, e);
    }

    private async Task AddSavedEndpoints(CancellationToken ct)
    {
        await foreach (var id in _activeEndpointsStorage.GetEndpointsAsync(ct))
        {
            var model = _repository.GetEndpoint(id); 
            if (model == null)
                return;
            var runtime = await _factory.CreateRuntimeAsync(model, CancellationToken.None);
            _scheduler.AddToSchedule(runtime);
        }
    }

    private async Task OnPingModelUpdated(EndpointModel model, CancellationToken ct)
    {
        if (!_scheduler.Endpoints.TryGetValue(model.Id, out var runtime)) //replace with cache check
            return;
        
        var update = await _updateResolver.ResolveAsync(model, CancellationToken.None);
        if (update is null)
            return;
        runtime.UpdateAddress(update);
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
        _scheduler.AddToSchedule(runtime);
        await _activeEndpointsStorage.SaveEndpointAsync(runtime.Id, ct);
    }

    public async Task RemoveEndpoint(string id, CancellationToken ct)
    {
        _scheduler.RemoveFromSchedule(id);
        await _activeEndpointsStorage.RemoveEndpointAsync(id, ct);
    }

    public bool IsRunning(string id) => _scheduler.Endpoints.ContainsKey(id); // Cache check
    
    public event EventHandler<string>? PingSent;

    public event EventHandler<PingUpdated>? PingReceived;
    public void Dispose()
    {
        foreach(var subscription in _subscriptions)
            subscription.Dispose();
    }
}