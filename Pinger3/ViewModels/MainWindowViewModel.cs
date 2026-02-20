using Pinger3.Models;
using Pinger3.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Pinger3.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {

        public ObservableCollection<IPingViewModel> ValidPingingTargets { get; }

        private readonly IAddressesConfigParser _parser;
        private readonly ResponeAwaitingTimeUpdaterService _timeUpdater;

        public MainWindowViewModel(IAddressesConfigParser parser, ResponeAwaitingTimeUpdaterService timeUpdaterService)
        {
            ValidPingingTargets = [];
            _timeUpdater = timeUpdaterService;
            _parser = parser;
        }
        
        public async Task LoadPingingTargets()
        {
            var addresses = await _parser.ParseConfigAsync();
            foreach (var address in addresses)
            {
                if(address.Item2 == ConfigValidationErrors.None)
                    ValidPingingTargets.Add(new PingViewModel(new PingTargetModel(address.Item1, new ICMPPinger(address.Item1)), _timeUpdater));
                //invalid entries for later
            }
        }

        public void StartPinging()
        {
            foreach (var target in ValidPingingTargets)
            {
                target.;
            }
        }

    }
}
