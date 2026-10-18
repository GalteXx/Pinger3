using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public sealed class EndpointRepository(IEndpointsStorage storage, IEndpointCache cache) : IEndpointRepository
{
    public async Task LoadFromSource()
    {
        var stream = storage.GetAddressesAsync(CancellationToken.None);
        await foreach (var endpoint in stream)
        {
            endpoint.Updated += (sender, _) =>
            {
                cache.AddOrUpdateValue(endpoint);
                if (sender is EndpointModel e)
                    OnEndpointUpdated(e.Id);
            };
        }
    }

    public async Task AddEndpointAsync(EndpointModel endpoint, CancellationToken ct)
    {
        await storage.AddEndpointAsync(endpoint, ct);
        cache.AddOrUpdateValue(endpoint);
        OnEndpointUpdated(endpoint.Id);
    }

    public async Task EditEndpointAsync(EndpointModel endpoint, CancellationToken ct)
    {
        await storage.EditEndpointAsync(endpoint, ct);
        cache.AddOrUpdateValue(endpoint);
        OnEndpointUpdated(endpoint.Id);
    }

    public EndpointModel? GetEndpoint(string id)
    {
        return cache.GetValueOrDefault(id);
    }

    private void OnEndpointUpdated(string id)
    {
        // Aggregator things
    }
}