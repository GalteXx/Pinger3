using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pinger3.Services;

/// <summary>
/// Loads .Xml storage with addresses at local/Geckosystem/Pinger
/// No point in abstracting it out, should not be used by other namespaces
/// </summary>
internal class XmlAddressesStorageLoader
{
    private const string ConfigName = "config.xml";

    private static readonly string DirectoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Geckosystem", "Pinger");

    public async Task<XDocument?> LoadStorageAsync()
    {
        Directory.CreateDirectory(DirectoryPath);
        var fullPath = Path.Combine(DirectoryPath, ConfigName);
        try
        {
            await using var stream = File.OpenRead(fullPath);
            return await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
        }
        catch
        {
            return null;
        }
    }

    public Task DisableXmlStorage()
    {
        var fullPath = Path.Combine(DirectoryPath, ConfigName);
        try
        {
            File.Move(fullPath, Path.Combine(DirectoryPath, "storage.xml.old"));
        }
        catch
        {
            // Ignored
        }

        return Task.CompletedTask;
    }

    public async Task SaveAsync(XDocument storage)
    {
        Directory.CreateDirectory(DirectoryPath);
        var fullPath = Path.Combine(DirectoryPath, ConfigName);
            await using var stream = File.Create(fullPath);
            await storage.SaveAsync(stream, SaveOptions.None, CancellationToken.None);
    }
}