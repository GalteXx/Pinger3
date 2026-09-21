using CommunityToolkit.Mvvm.Input;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Services.Pinging;
using Pinger3.ViewModels.Controls.Factories;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        public ObservableCollection<IPingingTargetViewModel> CurrentPingingTargetsGroup { get; } = [];

        private readonly IEndpointRepository _repository;
        private readonly IPingTargetViewModelFactory _factory;
        private readonly IPingCatalog _catalog;

        public MainWindowViewModel(IEndpointRepository endpointRepository, IPingTargetViewModelFactory factory,
            IPingCatalog catalog)
        {
            _repository = endpointRepository;
            _factory = factory;
            _catalog = catalog;
            _repository.EndpointUpdated += UpdateViewModel;

            foreach (var endpoint in _repository.CachedEndpoints.Values)
            {
                CurrentPingingTargetsGroup.Add(factory.Create(endpoint));
            }

            _catalog.PingSent += (_, id) =>
            {
                CurrentPingingTargetsGroup.FirstOrDefault(vm => vm.Id == id)?.OnPingSent();
            };
            _catalog.PingReceived += (_, update) =>
            {
                CurrentPingingTargetsGroup.FirstOrDefault(vm => vm.Id == update.Id)?.OnPingReceived(update);
            };
        }

        private void UpdateViewModel(object? sender, string e)
        {
            if (_repository.GetEndpoint(e) is not { } value)
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
        private async Task TogglePinging(string id, CancellationToken token)
        {
            await _catalog.ToggleEndpointPingingAsync(id, token);
        }
    }
}