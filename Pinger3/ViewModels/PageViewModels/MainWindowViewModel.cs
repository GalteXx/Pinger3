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
            => [.. _targetsVMs.PingingTargetsByCategory(SelectedGroupOfTargets)];

        [ObservableProperty]
        private PingingTargetCategory _selectedGroupOfTargets = PingingTargetCategory.ValidTargets;

        public INavigationViewModel NavigationVM { get; }

        private readonly ParsedTargetsViewModelsBuilder _targetsVMs;
        private readonly ResponseAwaitingTimeUpdaterService _timeUpdater;

        public MainWindowViewModel(ParsedTargetsViewModelsBuilder parser, 
            ResponseAwaitingTimeUpdaterService timeUpdaterService, INavigationViewModel navigationVM)
        {
            _timeUpdater = timeUpdaterService;
            _targetsVMs = parser;
            NavigationVM = navigationVM;
        }

        partial void OnSelectedGroupOfTargetsChanged(PingingTargetCategory value)
        {
            OnPropertyChanged(nameof(CurrentPingingTargetsGroup));
        }

        public async Task OnMainWindowLoaded()
        {
            _timeUpdater.Start();
            OnPropertyChanged(nameof(CurrentPingingTargetsGroup));
        }

        [RelayCommand]
        public void StartPinging()
        {
            //do nothing for now
        }

        [RelayCommand]
        public void StopPinging()
        {
            //do nothing for now
        }
    }
}
