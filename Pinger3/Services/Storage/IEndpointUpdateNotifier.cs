using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IEndpointUpdateNotifier
{
    IDisposable SubscribeToDeletion(Func<string, CancellationToken, ValueTask> handler);
    IDisposable SubscribeToUpdate(Func<EndpointModel, CancellationToken, ValueTask> handler);
}