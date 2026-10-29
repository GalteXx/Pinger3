using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.DataTypes;

public class AsyncEvent<T> : IAsyncEventSink<T>, IAsyncEventSource<T>
{
    private readonly HandlerList<T> _updates = new();

    public ValueTask PublishAsync(T args, CancellationToken ct = default)
    {
        return _updates.PublishAsync(args, ct);
    }

    public IDisposable Subscribe(Func<T, CancellationToken, ValueTask> handler)
    {
        return _updates.Subscribe(handler);
    }

    private sealed class HandlerList<THandler>
    {
        private readonly object _gate = new();
        private Func<THandler, CancellationToken, ValueTask>[] _handlers = [];

        public IDisposable Subscribe(Func<THandler, CancellationToken, ValueTask> handler)
        {
            lock (_gate) _handlers = [.. _handlers, handler];

            return new Subscription(() =>
            {
                lock (_gate)
                {
                    var i = Array.IndexOf(_handlers, handler);
                    if (i >= 0)
                        _handlers = [.. _handlers[..i], .. _handlers[(i + 1)..]];
                }
            });
        }

        public async ValueTask PublishAsync(THandler value, CancellationToken ct)
        {
            var snapshot = Volatile.Read(ref _handlers);
            foreach (var h in snapshot)
                await h(value, ct);
        }
    }

    private sealed class Subscription(Action dispose) : IDisposable
    {
        private Action? _dispose = dispose;
        public void Dispose() => Interlocked.Exchange(ref _dispose, null)?.Invoke();
    }
}