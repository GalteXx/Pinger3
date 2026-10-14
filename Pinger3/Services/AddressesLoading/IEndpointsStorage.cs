using Pinger3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IEndpointsStorage
{
    public IAsyncEnumerable<EndpointModel> ParseAddressesAsync();
        
    public Task WriteAddressAsync(EndpointModel address);
}