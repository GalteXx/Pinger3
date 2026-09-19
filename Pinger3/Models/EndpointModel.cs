using System;
using System.Net;
using System.Threading.Tasks;

namespace Pinger3.Models
{
    public sealed class EndpointModel(string id, string name, string address, TimeSpan delayBetweenRequests)
    {
        public string Id { get; } = id;

        public string Name { get; private set; } = name;

        public string Address { get; private set; } = address;

        public TimeSpan DelayBetweenRequests { get; private set; } = delayBetweenRequests;

        public event EventHandler? Updated;

        // this TOTALLY looks different from View Models. Something just got in your eye 
        public void Rename(string newName)
        {
            Name = newName;
            Updated?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeAddress(string newAddress)
        {
            Address = newAddress;
            Updated?.Invoke(this, EventArgs.Empty);
        }

        private void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}