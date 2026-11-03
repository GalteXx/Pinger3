using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.PopOut;

public interface IPopOutEventNotifier
{
    IDisposable SubscribeToRemoval(Func<string, CancellationToken, ValueTask> handler);
    IDisposable SubscribeToAddition(Func<EndpointModel, CancellationToken, ValueTask> handler);
}