using Pinger3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinger3.Services
{
    internal class XmlAddressStorage(IStorageGateway gateway, EndpointModelFactory factory)
        : IAddressesStorage
    {
        public async IAsyncEnumerable<EndpointModel> ParseAddressesAsync()
        {
            var dtoStream = gateway.ReadAddressAsync();

            await foreach (var endpointDto in dtoStream)
            {
                yield return factory.CreateValidEndpointConfig(endpointDto);
            }
        }

        public async Task WriteAddressAsync(EndpointModel address)
        {
            //this calls for extra factory
            await gateway.WriteAddressAsync(new EndpointDto(address.Id, address.Name, address.Address,
                address.DelayBetweenRequests.Milliseconds.ToString()));
        }
    }
}