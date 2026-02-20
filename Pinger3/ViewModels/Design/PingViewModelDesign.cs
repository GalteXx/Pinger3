using Pinger3.ViewModels.Controls;
using System;
using System.ComponentModel;

namespace Pinger3.ViewModels.Design
{
    public class PingViewModelDesign : IPingViewModel
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

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
