using Pinger3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinger3.Services;

internal class XmlEndpointStorage(IStorageGateway gateway, EndpointModelFactory factory)
    : IEndpointsStorage, IActiveEndpointsStorage, IPopOutEndpointsStorage
{
    public async IAsyncEnumerable<EndpointModel> ParseAddressesAsync()
    {
        var dtoStream = gateway.GetAllEndpointsAsync();

        await foreach (var endpointDto in dtoStream)
        {
            yield return factory.CreateValidEndpointConfig(endpointDto);
        }
    }

    public async Task WriteAddressAsync(EndpointModel address)
    {
        //this calls for extra factory
        await gateway.EditEndpointAsync(new EndpointDto(address.Id, address.Name, address.Address,
            address.DelayBetweenRequests.Milliseconds.ToString()));
    }

    public Task SaveActiveEndpointAsync(string id) => gateway.SaveActiveEndpointAsync(id);

    public Task RemoveActiveEndpointAsync(string id) => gateway.RemoveActiveEndpointAsync(id);

    public Task SavePopOutEndpointAsync(string id) => gateway.SavePopOutEndpointAsync(id);

    public Task RemovePopOutEndpointAsync(string id) => gateway.RemovePopOutEndpointAsync(id);
}