using CommunityToolkit.Mvvm.ComponentModel;
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

        public ObservableCollection<IPingingTargetViewModel> SelectableForPopoutTargetVMs 
            => [.. _targetsVmBuilder.PingingTargetsByCategory(PingingTargetCategory.ValidTargets)];

        public SettingsViewModel(IPopoutWindowViewModel popoutVM, ParsedTargetsViewModelsBuilder targetsVMBuilder)
        {
            _popoutVM = popoutVM;
            _targetsVmBuilder = targetsVMBuilder;
        }
    }
}
