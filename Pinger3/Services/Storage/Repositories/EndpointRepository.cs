using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public sealed class EndpointRepository(
    IEndpointsStorage storage,
    IEndpointsCache cache,
    IEndpointUpdatePublisher updatePublisher) : IEndpointRepository
{
    public IEnumerable<EndpointModel> GetEndpoints()
    {
        return cache.GetAllEndpoints();
    }

    public async Task AddEndpointAsync(EndpointModel endpoint, CancellationToken ct)
    {
        await storage.AddEndpointAsync(endpoint, ct);
        cache.AddOrUpdateValue(endpoint);
        await OnEndpointUpdated(endpoint);
    }

    public async Task EditEndpointAsync(EndpointModel endpoint, CancellationToken ct)
    {
        await storage.EditEndpointAsync(endpoint, ct);
        cache.AddOrUpdateValue(endpoint);
        await OnEndpointUpdated(endpoint);
    }

    public EndpointModel? GetEndpoint(string id)
    {
        return cache.GetValueOrDefault(id);
    }

    private async Task OnEndpointUpdated(EndpointModel model)
    {
        await updatePublisher.PublishUpdateAsync(model);
    }
}