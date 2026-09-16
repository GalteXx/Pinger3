using System;

namespace Pinger3.Models
{

    public struct AddressDTO(string name, string address, TimeSpan requestDelay)
    {
        public string Name { get; private set; } = name;
        public string Address { get; private set; } = address;
        public TimeSpan RequestDelay { get; private set; } = requestDelay;
    }
}