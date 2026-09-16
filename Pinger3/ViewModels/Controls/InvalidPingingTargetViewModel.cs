using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using System;
using System.ComponentModel;

namespace Pinger3.ViewModels.Controls
{
    public class InvalidPingingTargetViewModel : IPingingTargetViewModel
    {
        private ConfigValidationErrors validationErrors;

        public InvalidPingingTargetViewModel(EndpointConfig endpoint)
        {
            Name = endpoint.Name;
            DomainOrAddress = endpoint.Address;
            ValidationErrors = endpoint.ValidationErrors;
            DelayBetweenRequests = endpoint.RequestDelay;
        }


        public ConfigValidationErrors ValidationErrors
        {
            get => validationErrors; private set => validationErrors = value;
        }
        public string DomainOrAddress { get; private set; }

        public string Name { get; private set; }

        public TimeSpan Ping => TimeSpan.FromMilliseconds(-1d);

        public TimeSpan TimeSinceLastRequest => TimeSpan.FromMilliseconds(-1d);

        public bool IsActive { get => false; set { } }

        public IRelayCommand StartPingingCommand => new RelayCommand(() => { });

        public IRelayCommand StopPingingCommand => new RelayCommand(() => { });

        public IRelayCommand TogglePingingCommand => new RelayCommand(() => { });

        public TimeSpan DelayBetweenRequests { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
