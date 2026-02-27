using Pinger3.Models;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinger3.Services
{
    public class ParsedTargetsViewModelsBuilder
    {
        private readonly List<PingingTargetViewModel> _allParsedTargets;
        private readonly IAddressesConfigParser _configParser;
        private readonly ResponeAwaitingTimeUpdaterService _timeUpdater;


        public ParsedTargetsViewModelsBuilder(IAddressesConfigParser configParser, ResponeAwaitingTimeUpdaterService timeUpdater)
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
                var vm = new PingingTargetViewModel(new PingTargetModel(address), _timeUpdater, pinger);
                return vm;
            });

            var vms = await Task.WhenAll(creationTasks);
            _allParsedTargets.AddRange(vms);
        }

        public IEnumerable<IPingingTargetViewModel> PingingTargetsByCategory(PingingTargetCategory category)
        {
            return category switch
            {
                PingingTargetCategory.Everything => _allParsedTargets.Cast<IPingingTargetViewModel>(),
                PingingTargetCategory.ValidTargets => _allParsedTargets.Where(t => t.ValidationErrors == ConfigValidationErrors.None).Cast<IPingingTargetViewModel>(),
                PingingTargetCategory.InvalidTargets => _allParsedTargets.Where(t => t.ValidationErrors != ConfigValidationErrors.None).Cast<IPingingTargetViewModel>(),
                _ => _allParsedTargets.Cast<IPingingTargetViewModel>(),
            };
        }

    }
}
