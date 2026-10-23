using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public sealed class EndpointRepository(IEndpointsStorage storage, IEndpointsCache cache) : IEndpointRepository
{
    public IEnumerable<EndpointModel> GetEndpoints() => cache.GetAllEndpoints();

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