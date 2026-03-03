using Pinger3.Models;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinger3.Services
{
    public class ParsedTargetsViewModelsBuilder
    {
        private readonly List<IPingingTargetViewModel> _allParsedTargets;
        private readonly IAddressesConfigParser _configParser;
        private readonly ResponseAwaitingTimeUpdaterService _timeUpdater;

        public event EventHandler? ViewModelsBuilt; //i did not do INotifyPropertyChanged as initial design implies VMs to be static

        public ParsedTargetsViewModelsBuilder(IAddressesConfigParser configParser, ResponseAwaitingTimeUpdaterService timeUpdater)
        {
            _configParser = configParser;
            _timeUpdater = timeUpdater;
            _allParsedTargets = [];
        }

        public async Task ParseConfigAndCreateViewModelsAsync()
        {
            var addresses = await _configParser.ParseConfigAsync();

            var creationTasks = addresses.Select(async address =>
            {
                await Task.Yield();

                var pinger = new ICMPPinger(address);

                IPingingTargetViewModel vm;
                if (address.ValidationErrors == ConfigValidationErrors.None)
                    vm = new PingingTargetViewModel(new PingTargetModel(address), _timeUpdater, pinger);
                else
                    vm = new InvalidPingingTargetViewModel(address);
                return vm;
            });

            var vms = await Task.WhenAll(creationTasks);
            _allParsedTargets.AddRange(vms);
            ViewModelsBuilt?.Invoke(this, new EventArgs());
        }

        public void AddViewModelToParsedList(IPingingTargetViewModel newVm)
        {
            _allParsedTargets.Add(newVm);
            ViewModelsBuilt?.Invoke(this, new EventArgs());
        }

        public IEnumerable<IPingingTargetViewModel> PingingTargetsByCategory(PingingTargetCategory category)
        {
            return category switch
            {
                PingingTargetCategory.Everything => _allParsedTargets,
                PingingTargetCategory.ValidTargets => _allParsedTargets.Where(t => t.ValidationErrors == ConfigValidationErrors.None),
                PingingTargetCategory.InvalidTargets => _allParsedTargets.Where(t => t.ValidationErrors != ConfigValidationErrors.None),
                _ => _allParsedTargets,
            };
        }

    }
}
