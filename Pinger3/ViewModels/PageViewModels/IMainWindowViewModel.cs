using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public interface IMainWindowViewModel
    {
        public ObservableCollection<IPingViewModel> ValidPingingTargets { get; }
        public ObservableCollection<IPingViewModel> InvalidPingingTargets { get; }

        public IRelayCommand StartPingingCommand { get; }
        public IRelayCommand StopPingingCommand { get; }

    }
}