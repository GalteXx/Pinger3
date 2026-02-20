using System;
using System.Net;

namespace Pinger3.Models
{

    public struct AddressConfig(string name, string addressOrDomain, IPAddress iPAddresse, TimeSpan requestDelay, ConfigValidationErrors validationErrors)
    {
        public string Name { get; private set; } = name;
        public string AddressOrDomain { get; private set; } = addressOrDomain;
        public IPAddress ResolvedIpAddress { get; private set; } = iPAddresse;
        public TimeSpan RequestDelay { get; private set; } = requestDelay;
        public ConfigValidationErrors ValidationErrors { get; private set; } = validationErrors;
    }
}
