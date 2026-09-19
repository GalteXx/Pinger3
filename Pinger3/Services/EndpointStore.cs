using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public class EndpointStore(IAddressesStorageParser parser)
{
    private readonly Dictionary<string, EndpointModel> _endpoints = [];
    
    public ReadOnlyDictionary<string, EndpointModel> Endpoints => _endpoints.AsReadOnly();

    public event EventHandler<string>? EndpointUpdated;

    public async Task Load()
    {
        var stream = parser.ParseAddressesAsync();
        await foreach (var endpoint in stream)
        {
            _endpoints.Add(endpoint.Id, endpoint);
        }
    }

    protected virtual void OnEndpointUpdated(string e)
    {
        EndpointUpdated?.Invoke(this, e);
    }
}