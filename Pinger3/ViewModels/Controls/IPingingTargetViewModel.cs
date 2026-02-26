using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;

namespace Pinger3.ViewModels.Controls
{
    public interface IPingingTargetViewModel
    {
        string DomainOrAddress { get; }
        string Name { get; }
        TimeSpan Ping { get; }
        TimeSpan TimeSinceLastRequest { get; }

        TimeSpan DelayBetweenRequests { get; }

        bool IsActive { get; set; }

        public IRelayCommand StartPingingCommand { get; }
        public IRelayCommand StopPingingCommand { get; }
        public IRelayCommand TogglePingingCommand { get; }

        event PropertyChangedEventHandler? PropertyChanged;
    }
}