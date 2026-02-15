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
    internal class AddressesConfigParser : IAddressesConfigParser
    {

        public IEnumerable<AddressConfig> TargetIPAddresses => _targetIPAddresses;

        private readonly List<AddressConfig> _targetIPAddresses;
        private readonly List<AddressConfig> _invalidTargetIPAddresses;
        private const string _configName = "config.xml"; // Hardcode goes brrr

        public static async Task<AddressesConfigParser> CreateAsync()
        {
            var service = new AddressesConfigParser();
            return await service.InitializeAsync();
        }

        private async Task<AddressesConfigParser> InitializeAsync()
        {

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Geckosystem", "Pinger");

           Task<XDocument> doc = LoadConfigAsync(path);
            var fileStream = new FileStream(Path.Combine(path, _configName), FileMode.Open);
            await ValidateConfigAsync(await doc, fileStream);
            await ParseConfigAsync(await doc);
            return this;
        }
        public AddressesConfigParser()
        {
            _targetIPAddresses = [];
            _invalidTargetIPAddresses = [];
        }

        private static async Task<XDocument> LoadDefaultConfigAsync()
        {
            var assembly = typeof(AddressesConfigParser).Assembly;
            await using var stream =
                assembly.GetManifestResourceStream("Pinger_2.Assets.config.default.xml")
                ?? throw new InvalidOperationException("Default config not found.");

            return await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
        }


        private async Task<XDocument> LoadConfigAsync(string path)
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

        private static async Task ValidateConfigAsync(XDocument doc, FileStream fileStream)
        {
            if (doc.Root == null)
            {
                doc = await LoadDefaultConfigAsync();
            }

            var targets = doc.Root!.Element("TargetIPs");
            if (targets == null) return;

            foreach (var el in targets.Elements("TargetIP").ToList())
            {
                await ValidateConfigElement(targets, el);
            }
            await doc.SaveAsync(fileStream, SaveOptions.None, CancellationToken.None);
        }

        private static async Task<ConfigValidationErrors> ValidateConfigElement(XElement targets, XElement el)
        {
            ConfigValidationErrors errors = ConfigValidationErrors.None;
            if (el.Attribute("Name") == null)
            {
                el.Add(new XAttribute("Name", "AddressName"));
                errors |= ConfigValidationErrors.MissingName;
            }

            var ipAttr = el.Attribute("IP")?.Value;
            var domainAttr = el.Attribute("Domain")?.Value;

            //missing IP and Domain
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
                if(!await TryResolveDomain(el, domainAttr))
                    errors |= ConfigValidationErrors.DomainUnresolvable;
            }
            return errors;
        }

        private static async Task<bool> TryResolveDomain(XElement el, string domainAttr)
        {
            try
            {
                await Dns.GetHostAddressesAsync(domainAttr);
            }
            catch
            {
                el.ReplaceWith(new XComment($"Entry invalid: Failed to resolve domain {domainAttr}"));
            }
            return false;
        }

        private async Task ParseConfigAsync(XDocument doc)
        {
            _targetIPAddresses.Clear();

            var targets = doc.Root?.Element("TargetIPs");
            if (targets == null) return;

            foreach (var el in targets.Elements("TargetIP"))
            {
                var ipAttr = el.Attribute("IP")?.Value;
                var domainAttr = el.Attribute("Domain")?.Value;

                if (!string.IsNullOrWhiteSpace(ipAttr) && IPAddress.TryParse(ipAttr, out var ip))
                {
                    _targetIPAddresses.Add(
                        new AddressConfig(el.Attribute("Name")!.Value,
                                          ip.ToString(),
                                          ip));
                }
                else if (!string.IsNullOrWhiteSpace(domainAttr))
                {
                    try
                    {
                        var resolved = await Dns.GetHostAddressesAsync(domainAttr);
                        _targetIPAddresses.Add(
                            new AddressConfig(el.Attribute("Name")!.Value,
                                              el.Attribute("Domain")!.Value,
                                              resolved[0])); //to be made configurable
                    }
                    catch
                    { }
                }
            }
        }

    }
}
