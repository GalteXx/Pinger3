using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public sealed class EndpointRepository(IAddressesStorage storage) : IEndpointRepository
{
    private readonly Dictionary<string, EndpointModel> _endpoints = [];

    public ReadOnlyDictionary<string, EndpointModel> CachedEndpoints => _endpoints.AsReadOnly();

    public event EventHandler<string>? EndpointUpdated;

    public async Task Load()
    {
        var stream = storage.ParseAddressesAsync();
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

    public async Task Add(EndpointModel endpoint)
    {
        _endpoints.Add(endpoint.Id, endpoint);
        await storage.WriteAddressAsync(endpoint);
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