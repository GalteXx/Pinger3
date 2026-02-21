using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {

        public ObservableCollection<IPingingTargetViewModel> ValidPingingTargets { get; }
        public ObservableCollection<IPingingTargetViewModel> InvalidPingingTargets { get; }

        private readonly IAddressesConfigParser _parser;
        private readonly ResponeAwaitingTimeUpdaterService _timeUpdater;

        public MainWindowViewModel(IAddressesConfigParser parser, ResponeAwaitingTimeUpdaterService timeUpdaterService)
        {
            ValidPingingTargets = [];
            InvalidPingingTargets = [];
            _timeUpdater = timeUpdaterService;
            _parser = parser;
        }

        public async Task OnMainWindowLoaded()
        {
            _timeUpdater.Start();
            var addresses = await _parser.ParseConfigAsync();
            foreach (var address in addresses)
            {
                if (address.ValidationErrors == ConfigValidationErrors.None)
                    ValidPingingTargets.Add(new PingingTargetViewModel(new PingTargetModel(address), _timeUpdater, new ICMPPinger(address)));
                //invalid entries for later
            }
        }

        [RelayCommand]
        public void StartPinging()
        {
            foreach (var target in ValidPingingTargets)
            {
                (target as PingingTargetViewModel)!.StartPingingCommand.Execute(null);
            }
        }

        [RelayCommand]
        public void StopPinging()
        {
            foreach (var target in ValidPingingTargets)
            {
                (target as PingingTargetViewModel)!.StopPingingCommand.Execute(null);
            }

        }
    }
}
