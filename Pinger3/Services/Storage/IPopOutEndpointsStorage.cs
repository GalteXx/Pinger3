using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IPopOutEndpointsStorage
{
    Task SaveEndpointAsync(string id, CancellationToken ct);
    Task RemoveEndpointAsync(string id, CancellationToken ct);
    IAsyncEnumerable<string> GetAllPopOutEndpointsAsync(CancellationToken ct);
}