using System;
using System.Net;
using System.Threading.Tasks;

namespace Pinger3.Models
{
    public class EndpointModel(string id, string name, string address, TimeSpan delayBetweenRequests)
    {
        public string Id { get; } = id;
        public string Name { get; private set; } = name;

        public string Address { get; private set; } = address;

        public TimeSpan DelayBetweenRequests { get; } = delayBetweenRequests;

        public Task<IPAddress[]> ResolveAddressesAsync()
            => Dns.GetHostAddressesAsync(Address);
    }
}