using System;

namespace Pinger3.Services
{
    public interface IPingService
    {
        DateTime? LastRequestTime { get; }
        event EventHandler<TimeSpan>? PingReceived;
        void Start();
        void Stop();
    }
}
