using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Pinger3.Models;
using Pinger3.Services.Pinging;
using Pinger3.Services.PopOut;
using Pinger3.ViewModels.Controls.Factories;

namespace Pinger3.ViewModels.PageViewModels
{
    public sealed partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel, IDisposable
    {
        // TODO: get an observable Dictionary
        public ObservableCollection<IPingingTargetViewModel> Endpoints { get; } = [];

        private readonly TimeSpan _timerIntervalMs = TimeSpan.FromMilliseconds(10);
        private readonly IPingTargetViewModelFactory _factory;
        private readonly DispatcherTimer _timespanUpdateTimer;
        private readonly IPingManager _pingManager;
        private readonly IPopOutManager _popOutManager;
        private readonly List<IDisposable> _subscriptions;

        public MainWindowViewModel(IEndpointRepository endpointRepository, IPingTargetViewModelFactory factory,
            IPingManager pingManager, IPopOutManager popOutManager, IEndpointUpdateNotifier updateNotifier)
        {
            _factory = factory;
            _pingManager = pingManager;
            _popOutManager = popOutManager;
            _timespanUpdateTimer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = _timerIntervalMs,
                IsEnabled = true
            };
            _timespanUpdateTimer.Tick += UpdateEndpointViewModelTimeSpans;
            _timespanUpdateTimer.Start();

            foreach (var endpoint in endpointRepository.GetEndpoints())
            {
                Endpoints.Add(factory.Create(endpoint));
            }

            _subscriptions =
            [
                updateNotifier.SubscribeToUpdate(async (model, _) => await UpdateViewModel(model)),
                updateNotifier.SubscribeToDeletion(async (id, _) => await DeleteViewModel(id))
            ];

            _pingManager.PingSent += (_, id) => { Endpoints.FirstOrDefault(vm => vm.Id == id)?.OnPingSent(); };
            _pingManager.PingReceived += (_, update) =>
            {
                Endpoints.FirstOrDefault(vm => vm.Id == update.Id)?.OnPingReceived(update);
            };
        }

        private ValueTask<object> DeleteViewModel(string id)
        {
            var endpoint = Endpoints.FirstOrDefault(vm => vm.Id == id);
            if (endpoint != null)
                Endpoints.Remove(endpoint);
            return ValueTask.FromResult<object>(ValueTask.CompletedTask);
        }

        private void UpdateEndpointViewModelTimeSpans(object? sender, EventArgs e)
        {
            foreach (var endpoint in Endpoints)
            {
                endpoint.UpdateTimeSinceLastRequest();
            }
        }

        private ValueTask UpdateViewModel(EndpointModel model)
        {
            var vm = Endpoints.FirstOrDefault(vm => vm.Id == model.Id);
            if (vm == null)
            {
                Endpoints.Add(_factory.Create(model));
                return ValueTask.CompletedTask;
            }

            vm.UpdateFromModel(model);
            return ValueTask.CompletedTask;
        }

        public void OnMainWindowLoaded()
        {
            OnPropertyChanged(nameof(Endpoints));
        }

        [RelayCommand]
        private async Task TogglePopOut(string id, CancellationToken ct)
        {
            await _popOutManager.Toggle(id, ct);
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

        public void Dispose()
        {
            foreach (var subscription in _subscriptions)
                subscription.Dispose();
        }
    }
}