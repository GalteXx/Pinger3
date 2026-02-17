using Pinger3.Services;
using System;
using System.Net;

namespace Pinger3.Models
{
    public class PingTargetModel
    {
        public string Name { get; private set; }
        public string AddressOrDomain { get; private set; }
        public TimeSpan Ping { get; private set; }
        public DateTime? LastRequest { get; private set; }

        private readonly IPAddress _address;
        private readonly IPingService _pingService;

        public PingTargetModel(AddressConfig config, IPingService pingService)
        {
            Name = config.Name;
            AddressOrDomain = config.AddressOrDomain;
            _address = config.ResolvedIpAddress;

            Ping = TimeSpan.FromMilliseconds(-1);
            LastRequest = DateTime.MinValue;

            _pingService = pingService;
            _pingService.PingReceived += (sender, e) =>
            {
                Ping = e;
                LastRequest = DateTime.Now;
            };
        }
        public void StartPinging() => _pingService.Start();
        public void StopPinging() => _pingService.Stop();

    }
}
