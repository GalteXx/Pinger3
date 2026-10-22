using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IActiveEndpointsStorage
{
    Task SaveEndpointAsync(string id, CancellationToken ct);
    Task RemoveEndpointAsync(string id, CancellationToken ct);
    IAsyncEnumerable<string> GetEndpointsAsync(CancellationToken ct);
}