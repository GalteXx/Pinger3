using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IActiveEndpointsRegistry
{
    bool IsActive(string id);
    Task ActivateAsync(string id, CancellationToken ct);
    Task DeactivateAsync(string id, CancellationToken ct);
    IEnumerable<string> GetActiveIds();
}
