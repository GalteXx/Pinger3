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

    private static async Task<XDocument> LoadDefaultConfigAsync()
    {
        var assembly = typeof(XmlAddressesStorageLoader).Assembly;
        await using var stream =
            assembly.GetManifestResourceStream("Pinger3.Assets.config.default.xml")
            ?? throw new InvalidOperationException("Default config not found.");

        return await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
    }

    public async Task<XDocument> LoadStorageAsync()
    {
        Directory.CreateDirectory(DirectoryPath);
        var fullPath = Path.Combine(DirectoryPath, ConfigName);

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

    public async Task SaveAsync(XDocument storage)
    {
        Directory.CreateDirectory(DirectoryPath);
        var fullPath = Path.Combine(DirectoryPath, ConfigName);
        await using var createStream = File.Create(fullPath);
        await storage.SaveAsync(createStream, SaveOptions.None, CancellationToken.None);
    }
}