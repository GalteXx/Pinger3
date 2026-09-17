using Pinger3.Models;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services
{
    public sealed class IcmpPingService(EndpointEntry entry) : IPingService, IDisposable
    {
        private CancellationTokenSource? _cts;
        private DateTime _lastRequest = DateTime.Now;
        private bool _updateSuppressed = true;

        public event EventHandler<TimeSpan>? PingReceived;

        public DateTime? LastRequestTime => _updateSuppressed ? null : _lastRequest;


        public void Start()
        {
            if (_cts != null) 
                return;
            _cts = new CancellationTokenSource();
            _ = RunPingLoopAsync(_cts.Token);
        }
        public void Stop()
        {
            _cts?.Cancel();
            _cts = null;
        }

        private async Task RunPingLoopAsync(CancellationToken ct)
        {
            using var ping = new Ping();

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var rep = ping.SendPingAsync(entry, 2000);
                    OnPingSent();
                    var reply = await rep;
                    OnPingReceived(reply.Status == IPStatus.Success
                        ? TimeSpan.FromMilliseconds(reply.RoundtripTime)
                        : TimeSpan.FromMilliseconds(-1d)); //why is there no NaN for TimeSpan
                }
                catch
                {
                    OnPingReceived(TimeSpan.FromMilliseconds(-1d));
                }
                await Task.Delay(entry.RequestDelay, ct);
            }
        }

        private void OnPingSent()
        {
            _lastRequest = DateTime.Now;
            _updateSuppressed = false;
        }

        private void OnPingReceived(TimeSpan e)
        {
            PingReceived?.Invoke(this, e);
            _updateSuppressed = true;
        }

        public void Dispose()
        {
            Stop();
            _cts?.Dispose();
        }
    }
}