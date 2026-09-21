using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IPingCatalog
{
    Task AddModelAsync(string id, CancellationToken ct);
    void RemoveModel(string id);
    bool IsRunning(string id);
    event EventHandler<string>? PingSent;
    event EventHandler<PingUpdated>? PingReceived;
}