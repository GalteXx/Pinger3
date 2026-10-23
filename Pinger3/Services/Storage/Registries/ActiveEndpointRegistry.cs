using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public class ActiveEndpointRegistry(IActiveEndpointsCache cache, IActiveEndpointsStorage storage) : IActiveEndpointsRegistry
{
    public bool IsActive(string id) => cache.IsStored(id);

    public async Task ActivateAsync(string id, CancellationToken ct)
    {
        if (IsActive(id))
            return;
        await storage.SaveEndpointAsync(id, ct);
        cache.AddId(id);
    }

    public async Task DeactivateAsync(string id, CancellationToken ct)
    {
        if (!IsActive(id))
            return;
        // By the moment this called, the Endpoint is most likely gone already
        await storage.RemoveEndpointAsync(id, ct); 
        cache.RemoveId(id);
    }

    public IEnumerable<string> GetActiveIds() => cache.GetAllIds();
}