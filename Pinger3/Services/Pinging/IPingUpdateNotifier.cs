using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IPingUpdateNotifier
{
    IDisposable SubscribeToPingUpdated(Func<PingUpdated, CancellationToken, ValueTask> handler);
    IDisposable SubscribeToPingSent(Func<string, CancellationToken, ValueTask> handler);
}