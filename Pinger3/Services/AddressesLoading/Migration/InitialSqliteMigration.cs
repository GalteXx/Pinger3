using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
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
        var storage = await _loader.LoadStorageAsync();

        await using (var transaction = (SqliteTransaction)await connection.BeginTransactionAsync())
        {
            await CreateEndpointsTable(connection, transaction);
            await CreateEndpointRolesTable(connection, transaction);

            if (storage != null)
                await InsertEndpoints(connection, transaction, GetEndpoints(storage));

            await UpdateSqliteVersion(connection, transaction);
            await transaction.CommitAsync();
        }

        await _loader.DisableXmlStorage();
    }

    private static async Task InsertEndpoints(
        SqliteConnection connection, SqliteTransaction transaction, IEnumerable<XmlEndpoint> endpoints)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            "INSERT INTO endpoint(name, address, delay) VALUES ($name, $address, $delay)";

        var name = command.Parameters.Add("$name", SqliteType.Text);
        var address = command.Parameters.Add("$address", SqliteType.Text);
        var delay = command.Parameters.Add("$delay", SqliteType.Text);

        foreach (var e in endpoints)
        {
            name.Value = e.Name;
            address.Value = e.Address;
            delay.Value = e.Delay.ToString();
            await command.ExecuteNonQueryAsync();
        }
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
            var address = endpointElement.Attribute("IP")?.Value;
            if (string.IsNullOrWhiteSpace(address))
            {
                address = endpointElement.Attribute("Domain")?.Value;
                if (string.IsNullOrWhiteSpace(address))
                    address = "127.0.0.1";
            }

            var delayString = endpointElement.Attribute("Delay")?.Value;
            var delay = ParseDelay(delayString);

            yield return new XmlEndpoint(name, address, delay);
        }
    }

    private static async Task CreateEndpointRolesTable(SqliteConnection connection, SqliteTransaction transaction)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS endpoint_role (
                                role        TEXT NOT NULL CHECK (role IN ('active','popout')),
                                endpoint_id integer NOT NULL REFERENCES endpoint(id) ON DELETE CASCADE,
                                PRIMARY KEY (role, endpoint_id)
                              );
                              """;
        await command.ExecuteNonQueryAsync();
    }

    private static async Task CreateEndpointsTable(SqliteConnection connection, SqliteTransaction transaction)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
                              CREATE TABLE IF NOT EXISTS endpoint (
                                id        integer NOT NULL PRIMARY KEY,
                                name TEXT NOT NULL,
                                address TEXT NOT NULL,
                                delay TEXT NOT NULL
                              );
                              """;
        await command.ExecuteNonQueryAsync();
    }

    private async Task UpdateSqliteVersion(SqliteConnection connection, SqliteTransaction transaction)
    {
        var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = $"PRAGMA user_version = {ToVersion}";
        await command.ExecuteNonQueryAsync();
    }

    private static TimeSpan ParseDelay(string? s) =>
        int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ms) && ms > 0
            ? TimeSpan.FromMilliseconds(ms)
            : TimeSpan.FromSeconds(5);

    private record XmlEndpoint(string Name, string Address, TimeSpan Delay);
}