using System;
using System.Collections.Generic;
using Pinger3.Models;
using Pinger3.Services.Pinging;

namespace Pinger3.Services.PopOut;

public sealed class PopOutCatalog : IPopOutCatalog
{
    private readonly EndpointRepository _repository;
    private readonly HashSet<string> _ids;

    public PopOutCatalog(PingManager manager, EndpointRepository repository)
    {
        _repository = repository;
        _ids = [];

        repository.EndpointUpdated += OnEndpointUpdated;
        manager.PingReceived += (sender, updated) =>
        {
            if (IsPopOut(updated.Id))
                PingReceived?.Invoke(sender, updated);
        };
        manager.PingSent += PingSent;
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


    public void Add(string id)
    {
        if (_ids.Contains(id))
            return;

        var endpoint = _repository.GetEndpoint(id);

        if (endpoint == null)
            return;

        OnEndpointAdded(endpoint);
        _ids.Add(id);
    }

    public void Remove(string id)
    {
        OnEndpointRemoved(id);
    }

    public bool IsPopOut(string id)
    {
        return _ids.Contains(id);
    }

    private void OnEndpointAdded(EndpointModel model)
    {
        EndpointAdded?.Invoke(this, model);
    }

    private void OnEndpointRemoved(string id)
    {
        EndpointRemoved?.Invoke(this, id);
    }
}