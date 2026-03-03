using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.Design
{
    internal class SettingsViewModelDesign : ISettingsViewModel
    {
        public IPopoutWindowViewModel PopoutVM => new PopoutWindowViewModelDesign();

        public ObservableCollection<IPingingTargetWrapperViewModel> SelectableForPopoutTargetVMs => [new PingingTargetWrapperViewModelDesign(), new PingingTargetWrapperViewModelDesign()];
    }
}
