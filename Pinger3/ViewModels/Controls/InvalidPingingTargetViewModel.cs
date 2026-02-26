using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace Pinger3.ViewModels.Controls
{
    public class InvalidPingingTargetViewModel : IPingingTargetViewModel
    {
        private ConfigValidationErrors validationErrors;

        public InvalidPingingTargetViewModel(AddressConfig address)
        {
            Name = address.Name;
            DomainOrAddress = address.AddressOrDomain;
            ValidationErrors = address.ValidationErrors;
        }


        public ConfigValidationErrors ValidationErrors
        {
            get => validationErrors;
            private set
            {
                validationErrors = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationErrors)));

            }
        }
        public string DomainOrAddress { get; private set; }

        public string Name { get; private set; }

        public TimeSpan Ping => TimeSpan.FromMilliseconds(-1d);

        public TimeSpan TimeSinceLastRequest => TimeSpan.FromMilliseconds(-1d);

        public bool IsActive { get => false; set { } }

        public IRelayCommand StartPingingCommand => throw new NotSupportedException();

        public IRelayCommand StopPingingCommand => throw new NotSupportedException();

        public IRelayCommand TogglePingingCommand => throw new NotSupportedException();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
