using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class PingCatalog // TODO: rename catalog to something sane
    : IPingCatalog
{
    private readonly IPingScheduler _scheduler;
    private readonly EndpointRuntimeFactory _factory;
    private readonly EndpointRepository _repository;

    public PingCatalog(IPingScheduler scheduler, EndpointRuntimeFactory factory, EndpointRepository repository)
    {
        _scheduler = scheduler;
        _factory = factory;
        _repository = repository;

        scheduler.PingSent += PingSent;
        scheduler.PingReceived += PingReceived;
    }

    public async Task ToggleEndpointPingingAsync(string id, CancellationToken ct)
    {
        if(IsRunning(id))
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