using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class SettingsViewModel : ObservableObject, ISettingsViewModel, INotifyPropertyChanged
    {
        private readonly ParsedTargetsViewModelsBuilder _targetsVmBuilder;

        [ObservableProperty]
        private IPopoutWindowViewModel _popoutVM;

        public void ChangePopoutTargets(object changedTarget)
        {
            if (changedTarget is not IPingingTargetViewModel targetViewModel)
                return;
            if(PopoutVM.PingViewModels.Contains(targetViewModel)) //lol
                PopoutVM.PingViewModels.Add(targetViewModel);
            else
                PopoutVM.PingViewModels.Remove(targetViewModel);
        }

        public ObservableCollection<IPingingTargetViewModel> SelectableForPopoutTargetVMs
            => [.. _targetsVmBuilder.PingingTargetsByCategory(PingingTargetCategory.ValidTargets)];

        public SettingsViewModel(IPopoutWindowViewModel popoutVM, ParsedTargetsViewModelsBuilder targetsVMBuilder)
        {
            _popoutVM = popoutVM;
            _targetsVmBuilder = targetsVMBuilder;
        }
    }
}
