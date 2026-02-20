using Pinger3.Services;
using System;
using System.Net;

namespace Pinger3.Models
{
    public class PingTargetModel(AddressConfig config)
    {
        public string Name { get; private set; } = config.Name;
        public string AddressOrDomain { get; private set; } = config.AddressOrDomain;
        public TimeSpan Ping { get; set; } = TimeSpan.FromMilliseconds(-1);
        public DateTime? LastRequest { get; set; } = null;
    }
}
