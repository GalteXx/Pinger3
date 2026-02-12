using System.Net;

namespace Pinger3.Models
{
    public struct AddressConfig(string name, string addressOrDomain, IPAddress iPAddresse, int requestDelay)
    {
        public string Name { get; private set; } = name;
        public string AddressOrDomain { get; private set; } = addressOrDomain;
        public IPAddress ResolvedIpAddress { get; private set; } = iPAddresse;
        public int RequestDelay { get; private set; } = requestDelay;
    }
}
