using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IPingUpdatePublisher
{
    ValueTask PublishPingUpdateAsync(PingUpdated updatedModel, CancellationToken ct = default);
    ValueTask PublishPingSentAsync(string id, CancellationToken ct = default);
}