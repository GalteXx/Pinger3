using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using Pinger3.Services;
using System;

namespace Pinger3.ViewModels.Controls
{
    public partial class PingingTargetViewModel : ObservableObject, IPingingTargetViewModel
    {
        private readonly PingTargetModel _model;
        private readonly IPingService _pingService;

        public PingingTargetViewModel(PingTargetModel model, ResponseAwaitingTimeUpdaterService timeUpdater, IPingService pinger)
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

        [ObservableProperty]
        private string name = string.Empty;
        [ObservableProperty]
        private string domainOrAddress = string.Empty;
        [ObservableProperty]
        private TimeSpan ping;
        [ObservableProperty]
        private bool isActive;
        [ObservableProperty]
        private TimeSpan timeSinceLastRequest;

        public TimeSpan DelayBetweenRequests => _model.DelayBetweenRequests;

        public ConfigValidationErrors ValidationErrors => ConfigValidationErrors.None;

        partial void OnIsActiveChanged(bool value)
        {
            if(value)
                _pingService.Start();
            else
                _pingService.Stop();
        }


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
            if (IsActive)
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
    }
}
