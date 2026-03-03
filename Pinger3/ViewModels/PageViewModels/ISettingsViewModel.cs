using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public interface ISettingsViewModel
    {
        IPopoutWindowViewModel PopoutVM { get; }
        ObservableCollection<IPingingTargetWrapperViewModel> SelectableForPopoutTargetVMs { get; }
    }
}
