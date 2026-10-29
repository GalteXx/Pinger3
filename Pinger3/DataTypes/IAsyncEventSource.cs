using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.DataTypes;

public interface IAsyncEventSource<T>
{
    IDisposable Subscribe(Func<T, CancellationToken, ValueTask> handler);
}