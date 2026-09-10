using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Pinger3.Models;

namespace Pinger3.Services;

public class XmlAddressValidator
{
    public async IAsyncEnumerable<ConfigValidationErrors> ValidateAddressEntriesAsync(XDocument doc)
    {
        var targets = doc.Root!.Element("TargetIPs");
        if (targets == null) yield break;

        var elements = targets.Elements("TargetIP").ToList();

        foreach (var element in elements)
        {
            var errors = await ValidateConfigElement(element);
            yield return errors;
        }
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

    // At this point I want to make a metadata based validator, and use Attributes.
    // This looks horrible and I hate it
    private async Task<ConfigValidationErrors> ValidateConfigElement(XElement el)
    {
        var errors = ConfigValidationErrors.None;
        if (el.Attribute("Name") == null)
        {
            if (el.Attribute("Name") == null) // got rid of lock here. Might need to look up if I shouldn't have
            {
                el.Add(new XAttribute("Name", "AddressName"));
                errors |= ConfigValidationErrors.MissingName;
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
            if (!await TryResolveDomain(domainAttr)) //really not sure about this one
                errors |= ConfigValidationErrors.DomainUnresolvable; 
        }

        return errors;
    }
}