using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Pinger3.Models;

namespace Pinger3.Services;

public class XmlAddressValidator
{
    public async IAsyncEnumerable<AddressConfig>? ValidateAddressEntriesAsync(XDocument doc)
    {
        var targets = doc.Root!.Element("TargetIPs");
        if (targets == null) return null;

        var elements = targets.Elements("TargetIP").ToList();

        await foreach (var entry in elements)
        {
            var errors = await ValidateConfigElement(el);
            var config = await ParseValidatedConfigElement(el, errors);
            yield return config;
        }
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
}