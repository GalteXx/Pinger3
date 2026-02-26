using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public interface IMainWindowViewModel
    {
        public PingingTargetCategory SelectedGroupOfTargets { get; set; }
        public ObservableCollection<IPingingTargetViewModel> CurrentPingingTargetsGroup { get; }

        public IRelayCommand StartPingingCommand { get; }
        public IRelayCommand StopPingingCommand { get; }
        public INavigationViewModel NavigationVM { get; }
    }
}