using System;
using System.Net;
using System.Threading.Tasks;

namespace Pinger3.Models
{
    public class PingTargetModel(EndpointConfig config)
    {
        public string Name { get; private set; } = config.Name;

        public string Address { get; private set; } = config.Address;

        public TimeSpan Ping { get; set; } = TimeSpan.FromMilliseconds(-1);

        public DateTime? LastRequest { get; set; } = null;

        public TimeSpan DelayBetweenRequests { get; } = config.RequestDelay;

        public Task<IPAddress[]> ResolveAddressesAsync()
            => Dns.GetHostAddressesAsync(Address);
    }
}