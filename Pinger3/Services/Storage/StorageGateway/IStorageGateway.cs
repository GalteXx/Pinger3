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
    Task<EndpointDto?> GetEndpointAsync(string id, CancellationToken ct);
}