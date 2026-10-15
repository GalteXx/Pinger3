using Pinger3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinger3.Services;

internal class XmlEndpointStorage(IStorageGateway gateway, EndpointModelFactory factory)
    : IEndpointsStorage, IActiveEndpointsStorage, IPopOutEndpointsStorage
{
    public async IAsyncEnumerable<EndpointModel> GetAddressesAsync()
    {
        var dtoStream = gateway.GetAllEndpointsAsync();

        await foreach (var endpointDto in dtoStream)
        {
            yield return factory.CreateValidEndpointConfig(endpointDto);
        }
    }

    public async Task<EndpointModel?> GetEndpointAsync(string id)
    {
        var dto = await gateway.GetEndpoint(id);
        return dto == null ? null : factory.CreateValidEndpointConfig(dto);
    }

    public async Task EditEndpointAsync(EndpointModel endpoint)
    {
        await gateway.EditEndpointAsync(new EndpointDto(endpoint.Id, endpoint.Name, endpoint.Address,
            endpoint.DelayBetweenRequests.ToString()));
    }

    public async Task AddEndpointAsync(EndpointModel endpoint)
    {
        await gateway.AddEndpointAsync(new EndpointDto(endpoint.Id, endpoint.Name, endpoint.Address,
            endpoint.DelayBetweenRequests.ToString()));
    }

    public Task SaveActiveEndpointAsync(string id) => gateway.SaveActiveEndpointAsync(id);

    public Task RemoveActiveEndpointAsync(string id) => gateway.RemoveActiveEndpointAsync(id);

    public Task SavePopOutEndpointAsync(string id) => gateway.SavePopOutEndpointAsync(id);

    public Task RemovePopOutEndpointAsync(string id) => gateway.RemovePopOutEndpointAsync(id);
}