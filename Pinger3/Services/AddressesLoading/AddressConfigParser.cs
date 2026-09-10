using Pinger3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pinger3.Services
{
    internal class AddressStorageReader(XmlAddressesStorageLoader loader, XmlAddressValidator validator)
        : IAddressesConfigParser
    {
        private readonly XmlAddressesStorageLoader _loader = loader;
        private readonly XmlAddressValidator _validator = validator;
        private readonly object _docLock = new();

        private static async Task<bool> TryResolveDomain(string domainAttr)
        {
            try
            {
                await Dns.GetHostAddressesAsync(domainAttr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static async Task<AddressConfig> ParseValidatedConfigElement(XElement element,
            ConfigValidationErrors errors)
        {
            IPAddress resolvedIP;
            string addressOrDomain;
            string name = !errors.HasFlag(ConfigValidationErrors.MissingName)
                ? element.Attribute("Name")!.Value
                : "AddressName";

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

            return new AddressConfig(name, addressOrDomain, resolvedIP, delay, errors);
        }

        public async IAsyncEnumerable<AddressConfig> ParseAddressesAsync()
        {
            var storage = await _loader.LoadStorageAsync();
            
        }
    }
}