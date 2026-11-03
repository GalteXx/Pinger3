using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.PopOut;

public interface IPopOutEventPublisher
{
    ValueTask PublishAdditionAsync(EndpointModel model, CancellationToken ct);
    ValueTask PublishRemovalAsync(string id, CancellationToken ct);
}