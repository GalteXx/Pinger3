using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;
using Pinger3.Services.Pinging;

namespace Pinger3.Services.PopOut;

public sealed class PopOutCatalog : IPopOutCatalog
{
    private readonly IEndpointRepository _repository;
    private readonly IPopOutEndpointsStorage _storage;
    private readonly HashSet<string> _ids;

    public PopOutCatalog(IPingManager manager, IEndpointRepository repository, IPopOutEndpointsStorage storage)
    {
        _repository = repository;
        _storage = storage;
        _ids = [];

        repository.EndpointUpdated += OnEndpointUpdated;
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

    private void OnEndpointUpdated(object? sender, string id)
    {
        var model = _repository.GetEndpoint(id);
        if (model != null)
            EndpointUpdated?.Invoke(this, model);
    }

    public event EventHandler<EndpointModel>? EndpointAdded;
    public event EventHandler<string>? EndpointRemoved;

    public event EventHandler<EndpointModel>? EndpointUpdated;
    public event EventHandler<PingUpdated>? PingReceived;
    public event EventHandler<string>? PingSent;

    public IEnumerable<EndpointModel> GetEndpoints()
    {
        return _ids.Select(endpoint => _repository.GetEndpoint(endpoint)).OfType<EndpointModel>();
    }
    
    public async Task Add(string id, CancellationToken ct)
    {
        if (_ids.Contains(id))
            return;

        var endpoint = _repository.GetEndpoint(id);

        if (endpoint == null)
            return;

        await OnEndpointAdded(endpoint);
        _ids.Add(id);
    }

    public async Task Toggle(string id, CancellationToken ct)
    {
        if (_ids.Contains(id))
            await Remove(id, ct);
        else
            await Add(id, ct);
    }

    public async Task Remove(string id, CancellationToken ct)
    {
        _ids.Remove(id);
        await OnEndpointRemoved(id, ct);
    }

    public bool IsPopOut(string id)
    {
        return _ids.Contains(id);
    }

    private async Task OnEndpointAdded(EndpointModel model, CancellationToken ct)
    {
        await _storage.SaveEndpointAsync(model.Id, ct);
        EndpointAdded?.Invoke(this, model);
    }

    private async Task OnEndpointRemoved(string id, CancellationToken ct)
    {
        await _storage.RemoveEndpointAsync(id, ct);
        EndpointRemoved?.Invoke(this, id);
    }
}