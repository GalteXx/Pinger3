using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class PingManager : IPingManager
{
    private readonly IPingScheduler _scheduler;
    private readonly EndpointRuntimeFactory _factory;
    private readonly IEndpointRepository _repository;
    private readonly EndpointUpdateResolver _updateResolver;
    private readonly IActiveEndpointsStorage _storage;

    public PingManager(IPingScheduler scheduler, EndpointRuntimeFactory factory, IEndpointRepository repository,
        EndpointUpdateResolver updateResolver, IActiveEndpointsStorage storage)
    {
        _scheduler = scheduler;
        _factory = factory;
        _repository = repository;
        _updateResolver = updateResolver;
        _storage = storage;

        // It won't throw anything. Trust me bro
        _repository.EndpointUpdated += (_, id) => OnPingModelUpdated(id).Start();

        scheduler.PingSent += (sender, e) => PingSent?.Invoke(sender, e);
        scheduler.PingReceived += (sender, e) => PingReceived?.Invoke(sender, e);
    }

    private async Task AddSavedEndpoints()
    {
        await foreach (var id in _storage.GetActiveEndpointsAsync())
        {
            var model = _repository.GetEndpoint(id);
            if (model == null)
                return;
            var runtime = await _factory.CreateRuntimeAsync(model, CancellationToken.None);
            _scheduler.Endpoints.Add(runtime.Id, runtime);
        }
    }

    private async Task OnPingModelUpdated(string id)
    {
        if (!_scheduler.Endpoints.TryGetValue(id, out var runtime))
            return;
        var model = _repository.GetEndpoint(id);
        if (model is null)
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
            await RemoveEndpoint(id);
    }

    public async Task AddEndpointAsync(string id, CancellationToken ct)
    {
        var model = _repository.GetEndpoint(id);
        if (model == null)
            return;
        var runtime = await _factory.CreateRuntimeAsync(model, ct);
        _scheduler.Endpoints.Add(runtime.Id, runtime);
        await _storage.SaveActiveEndpointAsync(runtime.Id);
    }

    public async Task RemoveEndpoint(string id)
    {
        _scheduler.Endpoints.Remove(id);
        await _storage.RemoveActiveEndpointAsync(id);
    }

    public bool IsRunning(string id) => _scheduler.Endpoints.ContainsKey(id);
    
    public event EventHandler<string>? PingSent;

    public event EventHandler<PingUpdated>? PingReceived;
}