using Pinger3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinger3.Services
{
    public interface IAddressesConfigParser
    {
        public Task<IEnumerable<(AddressConfig, ConfigValidationErrors)>> ParseConfigAsync();
    }
}
