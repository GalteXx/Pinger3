using Pinger3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinger3.Services
{
    internal interface IAddressesStorageParser
    {
        public IAsyncEnumerable<EndpointConfig> ParseAddressesAsync();
    }
}
