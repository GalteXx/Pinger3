using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.Design
{
    public partial class MainWindowViewModelDesign : IMainWindowViewModel
    {
        public ObservableCollection<IPingingTargetViewModel> ValidPingingTargets { get; }

        public ObservableCollection<IPingingTargetViewModel> InvalidPingingTargets { get; }
        

        public ObservableCollection<IPingingTargetViewModel> CurrentPingingTargetsGroup => ValidPingingTargets;

        public PingingTargetCategory SelectedGroupOfTargets { get => PingingTargetCategory.ValidTargets; set { } }

        public INavigationViewModel NavigationVM => new NavigationViewModelDesign();

        [RelayCommand]
        private void StartPinging()
        { }
        [RelayCommand]
        private void StopPinging()
        { }

        public MainWindowViewModelDesign()
        {
            ValidPingingTargets = [];
            for (int i = 0; i < 4; i++)
            {
                ValidPingingTargets.Add(new PingingTargetViewModelDesign());
            }
            InvalidPingingTargets = [];
        }
    }
}
