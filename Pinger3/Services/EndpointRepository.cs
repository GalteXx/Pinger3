using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public sealed class EndpointRepository(IEndpointsStorage storage) : IEndpointRepository
{
    private readonly Dictionary<string, EndpointModel> _endpoints = [];

    public ReadOnlyDictionary<string, EndpointModel> CachedEndpoints => _endpoints.AsReadOnly();

    public event EventHandler<string>? EndpointUpdated;

    public async Task LoadFromSource()
    {
        var stream = storage.GetAddressesAsync();
        await foreach (var endpoint in stream)
        {
            _endpoints.Add(endpoint.Id, endpoint);
            endpoint.Updated += (sender, _) =>
            {
                if (sender is EndpointModel e)
                    OnEndpointUpdated(e.Id);
            };
        }
    }

    public async Task AddEndpoint(EndpointModel endpoint)
    {
        _endpoints.Add(endpoint.Id, endpoint);
        await storage.AddEndpointAsync(endpoint);
        OnEndpointUpdated(endpoint.Id);
    }

    public async Task EditEndpoint(EndpointModel endpoint)
    {
        _endpoints[endpoint.Id] = endpoint;
        await storage.EditEndpointAsync(endpoint);
        OnEndpointUpdated(endpoint.Id);
    }

    public EndpointModel? GetEndpoint(string id)
    {
        return _endpoints.GetValueOrDefault(id);
    }

    private void OnEndpointUpdated(string id)
    {
        EndpointUpdated?.Invoke(this, id);
    }
}