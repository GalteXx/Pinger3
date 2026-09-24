using System;
using Pinger3.Models;
using Pinger3.Services.Pinging;

namespace Pinger3.Services.PopOut;

public sealed class PopOutCatalog
{
    private readonly EndpointRepository _repository;

    public PopOutCatalog(PingManager manager, EndpointRepository repository)
    {
        _repository = repository;
        
        repository.EndpointUpdated += EndpointUpdated;
        manager.PingReceived += PingReceived;
        manager.PingSent += PingSent;
    }

    public event EventHandler<EndpointModel>? EndpointAdded;
    public event EventHandler<string>? EndpointRemoved;
    
    public event EventHandler<string>? EndpointUpdated;
    public event EventHandler<PingUpdated>? PingReceived;
    public event EventHandler<string>? PingSent;


    public void Add(string id)
    {
        var endpoint = _repository.GetEndpoint(id);
        if (endpoint != null)
            OnEndpointAdded(endpoint);
    }

    public void Remove(string id)
    {
        OnEndpointRemoved(id);
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