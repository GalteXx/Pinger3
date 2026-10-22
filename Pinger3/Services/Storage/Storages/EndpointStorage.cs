using Pinger3.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

internal class EndpointStorage(IStorageGateway gateway, EndpointModelFactory factory) : IEndpointsStorage
{
    public async IAsyncEnumerable<EndpointModel> GetAddressesAsync([EnumeratorCancellation] CancellationToken ct)
    {
        var dtoStream = gateway.GetAllEndpointsAsync(ct);

        await foreach (var endpointDto in dtoStream)
        {
            yield return factory.CreateValidEndpointModel(endpointDto);
        }
    }

    public async Task<EndpointModel?> GetEndpointAsync(string id, CancellationToken ct)
    {
        var dto = await gateway.GetEndpointAsync(id, ct);
        return dto == null ? null : factory.CreateValidEndpointModel(dto);
    }

    public async Task EditEndpointAsync(EndpointModel endpoint, CancellationToken ct)
    {
        await gateway.EditEndpointAsync(new EndpointDto(endpoint.Id, endpoint.Name, endpoint.Address,
            endpoint.DelayBetweenRequests.ToString()), ct);
    }

    public async Task AddEndpointAsync(EndpointModel endpoint, CancellationToken ct)
    {
        await gateway.AddEndpointAsync(new EndpointDto(endpoint.Id, endpoint.Name, endpoint.Address,
            endpoint.DelayBetweenRequests.ToString()), ct);
    }
}