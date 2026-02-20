using System.Collections.ObjectModel;

namespace Pinger3.ViewModels
{
    public interface IMainWindowViewModel
    {
        ObservableCollection<IPingViewModel> ValidPingingTargets { get; }
        ObservableCollection<IPingViewModel> InvalidPingingTargets { get; }
    }
}