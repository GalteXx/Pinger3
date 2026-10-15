using Pinger3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IEndpointsStorage
{
    IAsyncEnumerable<EndpointModel> GetAddressesAsync();
    Task EditEndpointAsync(EndpointModel endpoint);
    Task<EndpointModel?> GetEndpointAsync(string id);
    Task AddEndpointAsync(EndpointModel endpoint);
}