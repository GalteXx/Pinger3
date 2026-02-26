using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using System;
using System.ComponentModel;

namespace Pinger3.ViewModels.Design
{
    public class PingViewModelDesign : IPingingTargetViewModel
    {
        public PingViewModelDesign()
        {
            Ping = TimeSpan.FromMilliseconds(Random.Shared.Next(1, 120));
            TimeSinceLastRequest = TimeSpan.FromMilliseconds(Random.Shared.Next(1, 100));
        }

        public string DomainOrAddress => "192.168.2.1";

        public string Name => "Mockup Server";

        public TimeSpan Ping { get; }
        public TimeSpan TimeSinceLastRequest { get; }

        public bool IsActive
        {
            get
            {
                return Random.Shared.Next(0, 2) == 0;
            }
            set { }
        }

        public IRelayCommand StartPingingCommand => throw new NotImplementedException();

        public IRelayCommand StopPingingCommand => throw new NotImplementedException();

        public IRelayCommand TogglePingingCommand => throw new NotImplementedException();

        public TimeSpan DelayBetweenRequests => TimeSpan.FromMilliseconds(Random.Shared.Next(30, ((int)TimeSpan.FromHours(2).TotalMilliseconds)));

#pragma warning disable CS0067 // The event is never used
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
