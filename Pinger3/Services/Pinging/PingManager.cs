using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class PingManager : IPingManager
{
    private readonly IPingScheduler _scheduler;
    private readonly EndpointRuntimeFactory _factory;
    private readonly EndpointRepository _repository;
    private readonly EndpointUpdateResolver _updateResolver;

    public PingManager(IPingScheduler scheduler, EndpointRuntimeFactory factory, EndpointRepository repository,
        EndpointUpdateResolver updateResolver)
    {
        _scheduler = scheduler;
        _factory = factory;
        _repository = repository;
        _updateResolver = updateResolver;

        // It won't throw anything. Trust me bro
        _repository.EndpointUpdated += (sender, id) => OnPingModelUpdated(sender, id).Start();

        scheduler.PingSent += PingSent;
        scheduler.PingReceived += PingReceived;
    }

    private async Task OnPingModelUpdated(object? sender, string id)
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
        if (IsRunning(id))
            await AddEndpointAsync(id, ct);
        else
            RemoveEndpoint(id);
    }

    public async Task AddEndpointAsync(string id, CancellationToken ct)
    {
        var model = _repository.GetEndpoint(id);
        if (model == null)
            return;
        var runtime = await _factory.CreateRuntimeAsync(model, ct);
        _scheduler.Endpoints.Add(runtime.Id, runtime);
    }

    public void RemoveEndpoint(string id)
    {
        _scheduler.Endpoints.Remove(id);
    }

    public bool IsRunning(string id) => _scheduler.Endpoints.ContainsKey(id);

    public event EventHandler<string>? PingSent;

    public event EventHandler<PingUpdated>? PingReceived;
}