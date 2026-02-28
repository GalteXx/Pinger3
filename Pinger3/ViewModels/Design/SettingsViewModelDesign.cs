using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.Design
{
    internal class SettingsViewModelDesign : ISettingsViewModel
    {
        public IPopoutWindowViewModel PopoutVM => new PopoutWindowViewModelDesign();

        public ObservableCollection<IPingingTargetViewModel> SelectableForPopoutTargetVMs => [new PingingTargetViewModelDesign(),new PingingTargetViewModelDesign()];
    }
}
