using System;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Pinger3.Services.Pinging;
using Pinger3.ViewModels.Controls.Factories;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        // TODO: get an observable Dictionary
        public ObservableCollection<IPingingTargetViewModel> Endpoints { get; } = [];

        private readonly TimeSpan _timerIntervalMs = TimeSpan.FromMilliseconds(200);
        private readonly IEndpointRepository _repository;
        private readonly IPingTargetViewModelFactory _factory;
        private readonly DispatcherTimer _timespanUpdateTimer;
        private readonly IPingManager _pingManager;

        public MainWindowViewModel(IEndpointRepository endpointRepository, IPingTargetViewModelFactory factory,
            IPingManager pingManager)
        {
            _repository = endpointRepository;
            _factory = factory;
            _pingManager = pingManager;
            _repository.EndpointUpdated += UpdateViewModel;
            _timespanUpdateTimer = new DispatcherTimer(DispatcherPriority.Default)
            {
                Interval = _timerIntervalMs,
                IsEnabled = true
            };
            _timespanUpdateTimer.Tick += UpdateEndpointViewModels;
            _timespanUpdateTimer.Start();

            foreach (var endpoint in _repository.CachedEndpoints.Values)
            {
                Endpoints.Add(factory.Create(endpoint));
            }

            _pingManager.PingSent += (_, id) =>
            {
                Endpoints.FirstOrDefault(vm => vm.Id == id)?.OnPingSent();
            };
            _pingManager.PingReceived += (_, update) =>
            {
                Endpoints.FirstOrDefault(vm => vm.Id == update.Id)?.OnPingReceived(update);
            };
        }

        private void UpdateEndpointViewModels(object? sender, EventArgs e)
        {
            foreach (var endpoint in Endpoints)
            {
                endpoint.UpdateTimeSinceLastRequest(_timerIntervalMs);
            }
        }

        private void UpdateViewModel(object? sender, string e)
        {
            if (_repository.GetEndpoint(e) is not { } value)
                return;

            var vm = Endpoints.FirstOrDefault(vm => vm.Id == e);
            if (vm == null)
            {
                Endpoints.Add(_factory.Create(value));
                return;
            }

            vm.UpdateModel(value);
        }

        public void OnMainWindowLoaded()
        {
            OnPropertyChanged(nameof(Endpoints));
        }

        [RelayCommand]
        private async Task TogglePinging(string id, CancellationToken token)
        {
            await _pingManager.ToggleEndpointPingingAsync(id, token);
            var vm = Endpoints.FirstOrDefault(vm => vm.Id == id);
            if (vm == null)
                return;
            vm.IsActive = _pingManager.IsRunning(id);
        }
    }
}