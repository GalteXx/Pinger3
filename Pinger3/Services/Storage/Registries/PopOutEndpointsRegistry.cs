using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public class PopOutEndpointsRegistry(IPopOutEndpointsStorage storage, IPopOutEndpointsCache cache) 
    : IPopOutEndpointsRegistry
{
    public bool IsPopOut(string id) => cache.IsStored(id);

    public async Task AddPopOutAsync(string id, CancellationToken ct)
    {
        if (IsPopOut(id))
            return;
        await storage.SaveEndpointAsync(id, ct);
        cache.AddId(id);
    }

    public async Task RemovePopOutAsync(string id, CancellationToken ct)
    {
        if (!IsPopOut(id))
            return;
        await storage.RemoveEndpointAsync(id, ct);
        cache.RemoveId(id);
    }

    public IEnumerable<string> GetPopOutIds() => cache.GetAllIds();
}