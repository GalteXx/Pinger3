using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IEndpointUpdatePublisher
{
    ValueTask PublishUpdateAsync(EndpointModel updatedModel, CancellationToken ct = default);
    ValueTask PublishDeletionAsync(string modelId, CancellationToken ct = default);
}