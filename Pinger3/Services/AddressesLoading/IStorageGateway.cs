using System.Collections.Generic;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IStorageGateway
{
    IAsyncEnumerable<EndpointDto> ReadAllEndpointsAsync();
    Task EditEndpointAsync(EndpointDto dto);
    Task AddEndpointAsync(EndpointDto dto);
    Task<EndpointDto?> ReadEndpoint(string id);
}