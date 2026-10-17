using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IActiveEndpointsStorage
{
    Task SaveActiveEndpointAsync(string id);
    Task RemoveActiveEndpointAsync(string id);
    IAsyncEnumerable<string> GetActiveEndpointsAsync(CancellationToken ct);
}