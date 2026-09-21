using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using Pinger3.ViewModels.Controls.Factories;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        public ObservableCollection<IPingingTargetViewModel> CurrentPingingTargetsGroup { get; } = [];

        private readonly IEndpointRepository _repository;
        private readonly IPingTargetViewModelFactory _factory;

        public MainWindowViewModel(IEndpointRepository endpointRepository, IPingTargetViewModelFactory factory)
        {
            _repository = endpointRepository;
            _factory = factory;
            _repository.EndpointUpdated += UpdateViewModel;

            foreach (var endpoint in _repository.CachedEndpoints.Values)
            {
                CurrentPingingTargetsGroup.Add(factory.Create(endpoint));
            }
        }

        private void UpdateViewModel(object? sender, string e)
        {
            //Maybe I should have had model as event args?
            if (_repository.CachedEndpoints.GetValueOrDefault(e) is not { } value)
                return;
            
            var vm = CurrentPingingTargetsGroup.FirstOrDefault(vm => vm.Id == e);
            if (vm == null)
            {
                CurrentPingingTargetsGroup.Add(_factory.Create(value));
                return;
            }
            vm.UpdateModel(value);
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