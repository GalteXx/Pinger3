using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IPingManager
{
    Task AddEndpointAsync(string id, CancellationToken ct);
    Task RemoveEndpoint(string id, CancellationToken ct);
    bool IsRunning(string id);
    Task ToggleEndpointPingingAsync(string id, CancellationToken ct);
}