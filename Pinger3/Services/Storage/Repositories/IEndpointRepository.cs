using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IEndpointRepository
{
    Task AddEndpointAsync(EndpointModel endpoint, CancellationToken ct);
    EndpointModel? GetEndpoint(string id);
    Task EditEndpointAsync(EndpointModel endpoint, CancellationToken ct);
    IEnumerable<EndpointModel> GetEndpoints();
}