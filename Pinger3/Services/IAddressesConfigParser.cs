using Pinger3.Models;
using System.Collections.Generic;

namespace Pinger3.Services
{
    public interface IAddressesConfigParser
    {
        public IEnumerable<AddressConfig> TargetIPAddresses { get; }
    }
}
