using Pinger3.Models;
using Pinger3.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pinger3.ViewModels
{
    public class PingViewModel : INotifyPropertyChanged
    {
        PingTargetModel _model;
        public PingViewModel(AddressConfig targetConfig)
        {
            _model = new PingTargetModel(targetConfig, new ICMPPinger(targetConfig));

            Name = _model.Name;
            DomainOrAddress = _model.AddressOrDomain;
        }
        private string name;
        private string domainOrAddress;
        private TimeSpan ping;
        private TimeSpan timeSinceLastRequest;

        public string Name
        {
            get => name;
            private set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string DomainOrAddress
        {
            get => domainOrAddress;
            private set
            {
                if (domainOrAddress != value)
                {
                    domainOrAddress = value;
                    OnPropertyChanged();
                }
            }
        }
        public TimeSpan Ping
        {
            get => ping;
            set
            {
                if (ping != value)
                {
                    ping = value;
                    OnPropertyChanged();
                }
            }
        }
        public TimeSpan TimeSinceLastRequest
        {
            get => timeSinceLastRequest;
            set
            {
                if (timeSinceLastRequest != value)
                {
                    timeSinceLastRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string callerName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }
    }
}
