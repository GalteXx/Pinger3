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
    public class AddressesConfigParser : IAddressesConfigParser
    {
        private readonly object _docLock = new();
        private const string _configName = "config.xml"; // Hardcode goes brrr

        public async Task<IEnumerable<AddressConfig>> ParseConfigAsync()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Geckosystem", "Pinger");

            Task<XDocument> doc = LoadConfigAsync(path);

            return await ValidateConfigAsync(await doc);
        }
        public AddressesConfigParser()
        { }

        private static async Task<XDocument> LoadDefaultConfigAsync()
        {
            var assembly = typeof(AddressesConfigParser).Assembly;
            await using var stream =
                assembly.GetManifestResourceStream("Pinger3.Assets.config.default.xml")
                ?? throw new InvalidOperationException("Default config not found.");

            return await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
        }


        private static async Task<XDocument> LoadConfigAsync(string path)
        {
            Directory.CreateDirectory(path);
            string fullPath = Path.Combine(path, _configName);

            if (!File.Exists(fullPath))
            {
                var defaultConfig = await LoadDefaultConfigAsync();
                await using var createStream = File.Create(fullPath);
                await defaultConfig.SaveAsync(createStream, SaveOptions.None, CancellationToken.None);
                return defaultConfig;
            }
            try
            {
                await using var stream = File.OpenRead(fullPath);
                return await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
            }
            catch
            {
                var defaultConfig = await LoadDefaultConfigAsync();
                await using var overwrite = File.Create(fullPath);
                await defaultConfig.SaveAsync(overwrite, SaveOptions.None, CancellationToken.None);
                return defaultConfig;
            }
        }

        private async Task<IEnumerable<AddressConfig>> ValidateConfigAsync(XDocument doc)
        {
            var targets = doc.Root!.Element("TargetIPs");
            if (targets == null) return [];

            var elements = targets.Elements("TargetIP").ToList();

            var tasks = elements.Select(async el =>
            {
                var errors = await ValidateConfigElement(el);
                var config = await ParseValidatedConfigElement(el, errors);
                return config;
            });

            var results = await Task.WhenAll(tasks);
            return results;
        }

        private async Task<ConfigValidationErrors> ValidateConfigElement(XElement el)
        {
            ConfigValidationErrors errors = ConfigValidationErrors.None;
            if (el.Attribute("Name") == null)
            {
                lock (_docLock)
                {
                    if (el.Attribute("Name") == null)
                    {
                        el.Add(new XAttribute("Name", "AddressName"));
                        errors |= ConfigValidationErrors.MissingName;
                    }
                }
            }

            var ipAttr = el.Attribute("IP")?.Value;
            var domainAttr = el.Attribute("Domain")?.Value;

            if (string.IsNullOrWhiteSpace(ipAttr) && string.IsNullOrWhiteSpace(domainAttr))
            {
                errors |= ConfigValidationErrors.MissingAddress;
            }

            if (!string.IsNullOrWhiteSpace(ipAttr))
            {
                if (!IPAddress.TryParse(ipAttr, out _))
                {
                    errors |= ConfigValidationErrors.InvalidIpFormat;
                }
            }
            if (!string.IsNullOrWhiteSpace(domainAttr))
            {
                if (!await TryResolveDomain(domainAttr))
                    errors |= ConfigValidationErrors.DomainUnresolvable;
            }
            return errors;
        }

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


        private static async Task<AddressConfig> ParseValidatedConfigElement(XElement element, ConfigValidationErrors errors)
        {
            IPAddress resolvedIP;
            string addressOrDomain;
            string name = !errors.HasFlag(ConfigValidationErrors.MissingName)
                ? element.Attribute("Name")!.Value : "AddressName";

            if (errors.HasFlag(ConfigValidationErrors.MissingAddress) ||
                errors.HasFlag(ConfigValidationErrors.InvalidIpFormat) ||
                errors.HasFlag(ConfigValidationErrors.DomainUnresolvable))
            {
                addressOrDomain = "InvalidAddress";
                resolvedIP = IPAddress.None;
            }
            else
            {
                addressOrDomain = element.Attribute("IP") is null ?
                    element.Attribute("Domain")!.Value : element.Attribute("IP")!.Value;
                resolvedIP = element.Attribute("IP") is null ?
                    (await Dns.GetHostAddressesAsync(element.Attribute("Domain")!.Value))[0]
                    : IPAddress.Parse(element.Attribute("IP")!.Value);
            }

            TimeSpan delay = element.Attribute("Delay") is null ? TimeSpan.FromSeconds(1) :
                    TimeSpan.FromMilliseconds(Convert.ToDouble(element.Attribute("Delay")!.Value));

            return new AddressConfig(name, addressOrDomain, resolvedIP, delay, errors);
        }

    }
}
