using System;

namespace Pinger3.Services
{
    public interface IPingService
    {
        TimeSpan PingWaitingSpan { get; }
        event EventHandler<TimeSpan> PingReceived;
        void Start();
        void Stop();
    }
}
