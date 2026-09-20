using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public class EndpointRepository(IAddressesStorage storage)
{
    private readonly Dictionary<string, EndpointModel> _endpoints = [];

    public ReadOnlyDictionary<string, EndpointModel> Endpoints => _endpoints.AsReadOnly();

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

    public async Task Save(EndpointModel endpoint)
    {
        _endpoints.Add(endpoint.Id, endpoint);
        await storage.WriteAddressAsync(endpoint);
        OnEndpointUpdated(endpoint.Id);
    }

    protected virtual void OnEndpointUpdated(string id)
    {
        EndpointUpdated?.Invoke(this, id);
    }
}