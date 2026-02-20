using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using Pinger3.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pinger3.ViewModels.Controls
{
    public partial class PingingTargetViewModel : INotifyPropertyChanged, IPingViewModel
    {
        private readonly PingTargetModel _model;
        private readonly IPingService _pingService;

        public PingingTargetViewModel(PingTargetModel model, ResponeAwaitingTimeUpdaterService timeUpdater, IPingService pinger)
        {
            TimeSinceLastRequest = TimeSpan.Zero;
            _model = model;
            _pingService = pinger;
            _pingService.PingReceived += (sender, e) =>
            {
                Ping = e;
            };

            timeUpdater.Ticked += (sender, e) =>
            {
                TimeSinceLastRequest = _pingService.LastRequestTime is null ?
                    TimeSinceLastRequest : DateTime.Now - (DateTime)_pingService.LastRequestTime;
            };
            Name = _model.Name;
            DomainOrAddress = _model.AddressOrDomain;
        }

        private string name = string.Empty;
        private string domainOrAddress = string.Empty;
        private TimeSpan ping;
        private bool isActive;
        private TimeSpan timeSinceLastRequest;


        [RelayCommand]
        private void StartPinging()
        {
            _pingService.Start();
            IsActive = true;
        }
        [RelayCommand]
        private void StopPinging()
        {
            _pingService.Stop();
            IsActive = false;
        }

        [RelayCommand]
        private void TogglePinging()
        {
            if (isActive)
            {
                _pingService.Stop();
                IsActive = false;
            }
            else
            {
                _pingService.Start();
                IsActive = true;
            }
        }

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
            private set
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
            private set
            {
                    timeSinceLastRequest = value;
                    OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get => isActive;
            private set
            {
                isActive = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string callerName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }
    }
}
