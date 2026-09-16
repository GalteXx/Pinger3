using Pinger3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pinger3.Services
{
    internal class XmlAddressStorageParser(IStorageReader reader, XmlAddressValidator validator)
        : IAddressesStorageParser
    {
        private readonly object _docLock = new();

        private static async Task<AddressConfig> ParseValidatedConfigElement(AddressDTO element,
            ConfigValidationErrors errors)
        {
            IPAddress resolvedIP;
            string addressOrDomain;

            if (errors.HasFlag(ConfigValidationErrors.MissingAddress) ||
                errors.HasFlag(ConfigValidationErrors.InvalidIpFormat) ||
                errors.HasFlag(ConfigValidationErrors.DomainUnresolvable))
            {
                addressOrDomain = "InvalidAddress";
                resolvedIP = IPAddress.None;
            }
            else
            {
                addressOrDomain = element.Attribute("IP") is null
                    ? element.Attribute("Domain")!.Value
                    : element.Attribute("IP")!.Value;
                resolvedIP = element.Attribute("IP") is null
                    ? (await Dns.GetHostAddressesAsync(element.Attribute("Domain")!.Value))[0]
                    : IPAddress.Parse(element.Attribute("IP")!.Value);
            }

            var delay = element.Attribute("Delay") is null
                ? TimeSpan.FromSeconds(1)
                : TimeSpan.FromMilliseconds(Convert.ToDouble(element.Attribute("Delay")!.Value));

            return new AddressConfig(name, addressOrDomain, delay, errors);
        }

        public async IAsyncEnumerable<AddressConfig> ParseAddressesAsync()
        {
            var dtoStream = reader.ReadAddressAsync();

            await foreach (var errors in validator.ValidateAddressEntriesAsync(storage))
            {
                yield return await ParseValidatedConfigElement(storage.Elements().ElementAt(++i), errors);
            }
            
        }
    }
}