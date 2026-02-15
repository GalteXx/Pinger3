using Pinger3.Models;
using System.Collections.Generic;

namespace Pinger3.Services
{
    internal interface IAddressesConfigParser
    {
        public IEnumerable<AddressConfig> TargetIPAddresses { get; }
    }
}
