using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IEndpointsStorage
{
    IAsyncEnumerable<EndpointModel> GetEndpointsAsync(CancellationToken ct);
    Task EditEndpointAsync(EndpointModel endpoint, CancellationToken ct);
    Task<EndpointModel?> GetEndpointAsync(string id, CancellationToken ct);
    Task AddEndpointAsync(EndpointModel endpoint, CancellationToken ct);
    Task DeleteEndpointAsync(string id, CancellationToken ct);
}