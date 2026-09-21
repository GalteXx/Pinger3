using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        public ObservableCollection<IPingingTargetViewModel> CurrentPingingTargetsGroup { get; } = [];

        [ObservableProperty] private PingingTargetCategory _selectedGroupOfTargets = PingingTargetCategory.ValidTargets;

        private readonly IEndpointRepository _repository;
        
        public MainWindowViewModel(IEndpointRepository endpointRepository)
        {
            _repository = endpointRepository;
            
        }

        partial void OnSelectedGroupOfTargetsChanged(PingingTargetCategory value)
        {
            OnPropertyChanged(nameof(CurrentPingingTargetsGroup));
        }

        public void OnMainWindowLoaded()
        {
            OnPropertyChanged(nameof(CurrentPingingTargetsGroup));
        }

        [RelayCommand]
        public void StartPinging(string Id)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public void StopPinging()
        {
            throw new NotImplementedException();
        }
    }
}