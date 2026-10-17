using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IStorageGateway
{
    IAsyncEnumerable<EndpointDto> GetAllEndpointsAsync(CancellationToken ct);
    Task EditEndpointAsync(EndpointDto dto, CancellationToken ct);
    Task AddEndpointAsync(EndpointDto dto, CancellationToken ct);
    Task<EndpointDto?> GetEndpoint(string id, CancellationToken ct);
    Task SaveActiveEndpointAsync(string id, CancellationToken ct);
    Task RemoveActiveEndpointAsync(string id, CancellationToken ct);
    Task SavePopOutEndpointAsync(string id, CancellationToken ct);
    Task RemovePopOutEndpointAsync(string id, CancellationToken ct);
    IAsyncEnumerable<string> GetAllPopOutEndpointsAsync(CancellationToken ct);
    IAsyncEnumerable<string> GetActiveEndpointsAsync(CancellationToken ct);
}