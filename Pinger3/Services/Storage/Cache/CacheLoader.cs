using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public class CacheLoader(
    IPopOutEndpointsCache popOutEndpointsCache,
    IActiveEndpointsCache activeEndpointsCache,
    IEndpointsCache endpointsCache,
    IPopOutEndpointsStorage popOutEndpointsStorage,
    IActiveEndpointsStorage activeEndpointsStorage,
    IEndpointsStorage storage) : ICacheLoader
{
    public async Task LoadApplicationCacheAsync(CancellationToken ct)
    {
        await LoadEndpointsCacheAsync(ct);
        await LoadPopOutCacheAsync(ct);
        await LoadActiveEndpointsCacheAsync(ct);
    }

    private async Task LoadEndpointsCacheAsync(CancellationToken ct)
    {
        await foreach (var endpoint in storage.GetEndpointsAsync(ct))
        {
            endpointsCache.AddOrUpdateValue(endpoint);
        }
    }

    private async Task LoadPopOutCacheAsync(CancellationToken ct)
    {
        await foreach (var popoutId in popOutEndpointsStorage.GetAllEndpointsAsync(ct))
        {
            popOutEndpointsCache.AddId(popoutId);
        }
    }

    private async Task LoadActiveEndpointsCacheAsync(CancellationToken ct)
    {
        await foreach (var activePopOutId in activeEndpointsStorage.GetEndpointsAsync(ct))
        {
            activeEndpointsCache.AddId(activePopOutId);
        }
    }
}