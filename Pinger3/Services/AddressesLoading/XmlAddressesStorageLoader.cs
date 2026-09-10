using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pinger3.Services;

internal class XmlAddressesStorageLoader : IAddressesStorageLoader<XDocument>
{
    private const string ConfigName = "config.xml";

    private static string DirectoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Geckosystem", "Pinger");

    private static async Task<XDocument> LoadDefaultConfigAsync()
    {
        var assembly = typeof(AddressStorageReader).Assembly;
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
}