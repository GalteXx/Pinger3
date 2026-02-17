using Pinger3.Models;
using Pinger3.Services;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        public ObservableCollection<IPingViewModel> PingTargets { get; }

        private readonly AddressesConfigParser _parser;
        private readonly ResponeAwaitingTimeUpdaterService _timeUpdater;

        public MainWindowViewModel()
        {
            //To be DI
            _parser = AddressesConfigParser.CreateAsync().Result;
            _timeUpdater = new ResponeAwaitingTimeUpdaterService();
            PingTargets = [];

            var addresses = _parser.TargetIPAddresses;
            foreach (var address in addresses)
            {
                PingTargets.Add(new PingViewModel(new PingTargetModel(address, new ICMPPinger(address)), _timeUpdater));
            }
        }
    }
}
