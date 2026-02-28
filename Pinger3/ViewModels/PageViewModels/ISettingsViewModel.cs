using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public interface ISettingsViewModel
    {
        IPopoutWindowViewModel PopoutVM { get; }
        ObservableCollection<IPingingTargetViewModel> SelectableForPopoutTargetVMs { get; }

        //IRelayCommand<object> ChangePopoutTargetsCommand { get; }
        void ChangePopoutTargets(object popoutTargets);
    }
}
