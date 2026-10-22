using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public class EndpointUpdateAggregator : IEndpointUpdateNotifier, IEndpointUpdatePublisher
{
    private readonly HandlerList<EndpointModel> _updates = new();
    private readonly HandlerList<string> _deletions = new();

    public IDisposable SubscribeToUpdate(Func<EndpointModel, CancellationToken, ValueTask> handler)
        => _updates.Subscribe(handler);

    public IDisposable SubscribeToDeletion(Func<string, CancellationToken, ValueTask> handler)
        => _deletions.Subscribe(handler);

    public ValueTask PublishUpdateAsync(EndpointModel updatedModel, CancellationToken ct = default)
        => _updates.PublishAsync(updatedModel, ct);

    public ValueTask PublishDeletionAsync(string modelId, CancellationToken ct = default)
        => _deletions.PublishAsync(modelId, ct);

    private sealed class HandlerList<T>
    {
        private readonly object _gate = new();
        private Func<T, CancellationToken, ValueTask>[] _handlers = [];

        public IDisposable Subscribe(Func<T, CancellationToken, ValueTask> handler)
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

        public async ValueTask PublishAsync(T value, CancellationToken ct)
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