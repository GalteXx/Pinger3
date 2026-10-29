using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IEndpointRuntimeFactory
{
    Task<EndpointRuntime> CreateRuntimeAsync(EndpointModel model, CancellationToken ct);
}