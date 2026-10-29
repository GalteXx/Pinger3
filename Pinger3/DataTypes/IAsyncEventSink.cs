using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.DataTypes;

public interface IAsyncEventSink<T>
{
    ValueTask PublishAsync(T args, CancellationToken ct = default);
}