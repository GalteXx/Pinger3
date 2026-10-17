using Pinger3.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IEndpointsStorage
{
    IAsyncEnumerable<EndpointModel> GetAddressesAsync(CancellationToken ct);
    Task EditEndpointAsync(EndpointModel endpoint, CancellationToken ct);
    Task<EndpointModel?> GetEndpointAsync(string id, CancellationToken ct);
    Task AddEndpointAsync(EndpointModel endpoint, CancellationToken ct);
}