using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class SettingsViewModel : ObservableObject, ISettingsViewModel, INotifyPropertyChanged
    {
        private readonly ParsedTargetsViewModelsBuilder _targetsVmBuilder;

        [ObservableProperty]
        private IPopoutWindowViewModel _popoutVM;

        public ObservableCollection<IPingingTargetWrapperViewModel> SelectableForPopoutTargetVMs { get; private set; }

        public SettingsViewModel(IPopoutWindowViewModel popoutVM, ParsedTargetsViewModelsBuilder targetsVMBuilder)
        {
            _popoutVM = popoutVM;
            _targetsVmBuilder = targetsVMBuilder;
            _targetsVmBuilder.ViewModelsBuilt += OnTargetVMsRebuilt;
            SelectableForPopoutTargetVMs = [];
        }

        private void WrapViewModels()
        {
            SelectableForPopoutTargetVMs = [.. _targetsVmBuilder.PingingTargetsByCategory(PingingTargetCategory.ValidTargets)
                .Select(x => new PingingTargetWrapperViewModel(x, PopoutVM))];

        }

        private void OnTargetVMsRebuilt(object? sender, EventArgs e)
        {
            WrapViewModels();
            OnPropertyChanged(nameof(SelectableForPopoutTargetVMs));
        }
    }
}
