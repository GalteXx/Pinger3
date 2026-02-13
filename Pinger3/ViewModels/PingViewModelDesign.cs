using System;
using System.ComponentModel;

namespace Pinger3.ViewModels
{
    public class PingViewModelDesign : IPingViewModel
    {
        public PingViewModelDesign() { }

        public string DomainOrAddress => "192.168.2.1";

        public string Name => "Mockup Server";

        public TimeSpan Ping => TimeSpan.FromMilliseconds(52);
        public TimeSpan TimeSinceLastRequest => TimeSpan.FromSeconds(6);

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
