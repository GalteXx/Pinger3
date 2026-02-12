using Pinger3.Services;
using System;
using System.Net;

namespace Pinger3.Models
{
    internal class PingTargetModel
    {
        public string Name { get; private set; }
        public string AddressOrDomain { get; private set; }
        public TimeSpan Ping { get; private set; }
        public TimeSpan TimeSinceLastRequest { get; private set; }

        private IPAddress _address;
        private IPingService _pingService;

        public PingTargetModel(AddressConfig config, IPingService pingService)
        {
            Name = config.Name;
            AddressOrDomain = config.AddressOrDomain;
            _address = config.ResolvedIpAddress;

            Ping = TimeSpan.FromMilliseconds(-1);
            TimeSinceLastRequest = TimeSpan.FromMilliseconds(-1);

            _pingService = pingService;
            _pingService.PingReceived += (sender, e) =>
            {
                Ping = e;
                TimeSinceLastRequest = TimeSpan.Zero;
                UpdateTimer();
            };
        }
        public void UpdateTimer() => TimeSinceLastRequest = _pingService.PingWaitingSpan;
        public void StartPinging() => _pingService.Start();
        public void StopPinging() => _pingService.Stop();

    }
}
