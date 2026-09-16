using System;
using System.Net;

namespace Pinger3.Models
{

    public struct EndpointConfig(string name, string address, TimeSpan requestDelay, ConfigValidationErrors validationErrors)
    {
        public string Name { get; private set; } = name;
        public string Address { get; private set; } = address;
        public TimeSpan RequestDelay { get; private set; } = requestDelay;
        public ConfigValidationErrors ValidationErrors { get; private set; } = validationErrors;
    }
}
