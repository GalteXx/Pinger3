using Pinger3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public Task WriteAddressAsync(EndpointModel address)
        {
            
        }
    }
}