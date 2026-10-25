using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IPopOutEndpointsRegistry // I have considered making generic MembershipRegistry, but decided not to
{
    bool IsPopOut(string id);
    Task AddPopOutAsync(string id, CancellationToken ct);
    Task RemovePopOutAsync(string id, CancellationToken ct);
    IEnumerable<string> GetPopOutIds();
}