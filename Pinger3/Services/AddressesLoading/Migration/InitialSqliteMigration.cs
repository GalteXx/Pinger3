using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;

namespace Pinger3.Services.Migration;

public class InitialSqliteMigration : IStorageMigration
{
    public int FromVersion => 0;
    public int ToVersion => 1;

    private readonly XmlAddressesStorageLoader _loader = new();

    public async Task Migrate(SqliteConnection connection)
    {
        await CreateEndpointsTable(connection);
        await CreateEndpointRolesTable(connection);

        await MigrateXmlAsync(connection);
    }

    private async Task MigrateXmlAsync(SqliteConnection connection)
    {
        var storage = await _loader.LoadStorageAsync();
        if (storage == null)
            return;
        var transaction = await connection.BeginTransactionAsync();
        try
        {
            foreach (var endpoint in GetEndpoints(storage))
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    $"INSERT INTO endpoint(name, address, delay) VALUES ('{endpoint.Name}', '{endpoint.Address}', '{endpoint.Delay.ToString()}'));";
                await command.ExecuteReaderAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        await _loader.DisableXmlStorage();
    }

    private IEnumerable<XmlEndpoint> GetEndpoints(XDocument xDocument)
    {
        var element = xDocument.Root?.Element("TargetIPs");
        if (element == null)
            yield break;
        foreach (var endpointElement in element.Elements())
        {
            var name = endpointElement.Attribute("Name")?.Value ?? "Missing Name";
            // I give priority to explicit Ip for no particular reason
            var address = endpointElement.Attribute("IP")?.Value ??
                          endpointElement.Attribute("Domain")?.Value ?? "127.0.0.1";
            var delayString = endpointElement.Attribute("Delay")?.Value;
            TimeSpan delay;
            try
            {
                delay = TimeSpan.FromMilliseconds(Convert.ToInt32(delayString));
            }
            catch
            {
                delay = TimeSpan.FromSeconds(5);
            }
            yield return new XmlEndpoint(name, address, delay);
        }
    }

    private static async Task CreateEndpointRolesTable(SqliteConnection connection)
    {
        var createRolesTable = connection.CreateCommand();
        createRolesTable.CommandText = """
                                       CREATE TABLE IF NOT EXISTS endpoint_role (
                                         role        TEXT NOT NULL CHECK (role IN ('active','popout')),
                                         endpoint_id TEXT NOT NULL REFERENCES endpoint(id),
                                         PRIMARY KEY (role, endpoint_id)
                                       );
                                       """;
        await createRolesTable.ExecuteNonQueryAsync();
    }

    private static async Task CreateEndpointsTable(SqliteConnection connection)
    {
        var createEndpointsTable = connection.CreateCommand();
        createEndpointsTable.CommandText = """
                                           CREATE TABLE IF NOT EXISTS endpoint (
                                             id        integer NOT NULL PRIMARY KEY,
                                             name TEXT NOT NULL,
                                             address TEXT NOT NULL,
                                             delay TEXT NOT NULL
                                           );
                                           """;
        await createEndpointsTable.ExecuteNonQueryAsync();
    }

    private record XmlEndpoint(string Name, string Address, TimeSpan Delay);
}