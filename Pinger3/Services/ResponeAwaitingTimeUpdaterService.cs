using Avalonia.Threading;
using System;


namespace Pinger3.Services
{
    public sealed class ResponeAwaitingTimeUpdaterService
    {
        private readonly TimeSpan TimerInterval = TimeSpan.FromMicroseconds(60);

        private readonly DispatcherTimer _timer;
        public event EventHandler? Ticked;

        public ResponeAwaitingTimeUpdaterService()
        {
            Ticked = new EventHandler((sender, e) => { });
            _timer = new DispatcherTimer(TimerInterval, DispatcherPriority.Render, Ticked);
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();
    }
}
