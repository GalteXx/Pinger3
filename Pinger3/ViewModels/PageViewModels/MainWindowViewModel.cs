using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        public ObservableCollection<IPingingTargetViewModel> CurrentPingingTargetsGroup
        {
            get
            {
                return SelectedGroupOfTargets switch
                {
                    PingingTargetCategory.ValidTargets => [.. ValidPingingTargets],
                    PingingTargetCategory.InvalidTargets => [.. InvalidPingingTargets],
                    PingingTargetCategory.Everything => [.. ValidPingingTargets.Concat(InvalidPingingTargets)],
                    _ => [],
                };
            }
        }
        [ObservableProperty]
        private PingingTargetCategory _selectedGroupOfTargets = PingingTargetCategory.ValidTargets;

        private readonly List<IPingingTargetViewModel> ValidPingingTargets;
        private readonly List<IPingingTargetViewModel> InvalidPingingTargets;
        private readonly IAddressesConfigParser _parser;
        private readonly ResponeAwaitingTimeUpdaterService _timeUpdater;

        public MainWindowViewModel(IAddressesConfigParser parser, ResponeAwaitingTimeUpdaterService timeUpdaterService)
        {
            ValidPingingTargets = [];
            InvalidPingingTargets = [];
            _timeUpdater = timeUpdaterService;
            _parser = parser;
        }

        partial void OnSelectedGroupOfTargetsChanged(PingingTargetCategory value)
        {
            OnPropertyChanged(nameof(CurrentPingingTargetsGroup));
        }

        public async Task OnMainWindowLoaded()
        {
            _timeUpdater.Start();
            var addresses = await _parser.ParseConfigAsync();
            foreach (var address in addresses)
            {
                if (address.ValidationErrors == ConfigValidationErrors.None)
                    ValidPingingTargets.Add(new PingingTargetViewModel(new PingTargetModel(address), _timeUpdater, new ICMPPinger(address)));
                else
                    InvalidPingingTargets.Add(new InvalidPingingTargetViewModel(address));
            }
            OnPropertyChanged(nameof(CurrentPingingTargetsGroup));
        }

        [RelayCommand]
        public void StartPinging()
        {
            foreach (var target in ValidPingingTargets)
            {
                (target as PingingTargetViewModel)!.StartPingingCommand.Execute(null);
            }
        }

        [RelayCommand]
        public void StopPinging()
        {
            foreach (var target in ValidPingingTargets)
            {
                (target as PingingTargetViewModel)!.StopPingingCommand.Execute(null);
            }

        }
    }
}
