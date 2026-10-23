using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface ICacheLoader
{
    Task LoadApplicationCacheAsync(CancellationToken ct);
}