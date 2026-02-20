using Avalonia.Threading;
using System;


namespace Pinger3.Services
{
    public sealed class ResponeAwaitingTimeUpdaterService
    {
        private readonly TimeSpan TimerInterval = TimeSpan.FromMilliseconds(16);

        private readonly DispatcherTimer _timer;
        public event EventHandler? Ticked;

        public ResponeAwaitingTimeUpdaterService()
        {
            Ticked = new EventHandler((sender, e) => { });
            _timer = new DispatcherTimer(TimerInterval, DispatcherPriority.Normal, Ticked);
            _timer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            Ticked!.Invoke(this, EventArgs.Empty);
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();
    }
}
