using Pinger3.Models;
using Pinger3.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Pinger3.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {

        public ObservableCollection<IPingViewModel> PingTargets { get; }

        private IAddressesConfigParser _parser;
        private readonly ResponeAwaitingTimeUpdaterService _timeUpdater;

        public MainWindowViewModel(IAddressesConfigParser parser, ResponeAwaitingTimeUpdaterService timeUpdaterService)
        {
            //To be DI 
            PingTargets = [];
            _timeUpdater = timeUpdaterService;
            _parser = parser;
        }

        public async Task InitializeAsync()
        {
            var addresses = _parser.TargetIPAddresses;
            foreach (var address in addresses)
            {
                PingTargets.Add(new PingViewModel(new PingTargetModel(address, new ICMPPinger(address)), _timeUpdater));
            }
        }
    }
}
