using System.Collections.Generic;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IStorageGateway
{
    IAsyncEnumerable<EndpointDto> GetAllEndpointsAsync();
    Task EditEndpointAsync(EndpointDto dto);
    Task AddEndpointAsync(EndpointDto dto);
    Task<EndpointDto?> GetEndpoint(string id);
    Task SaveActiveEndpointAsync(string id);
    Task RemoveActiveEndpointAsync(string id);
    Task SavePopOutEndpointAsync(string id);
    Task RemovePopOutEndpointAsync(string id);
}