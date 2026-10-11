using System.Collections.Generic;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public class SqliteStorageGateway(SqliteConnectionFactory connectionFactory)
    : IStorageGateway, IActiveEndpointsStorage, IPopOutEndpointsStorage
{
    public async IAsyncEnumerable<EndpointDto> ReadAddressesAsync()
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM endpoint";
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return new EndpointDto(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3));
        }
    }

    public async Task WriteAddressAsync(EndpointDto dto)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO endpoint WITH id VALUES (@id, @name)";
    }

    public Task WriteActiveEndpointAsync(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task EraseActiveEndpointAsync(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task WritePopOutEndpointAsync(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task ErasePopOutEndpointAsync(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task AddActiveEndpointAsync(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task RemoveActiveEndpointAsync(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task AddPopOutEndpointAsync(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task RemovePopOutEndpointAsync(string id)
    {
        throw new System.NotImplementedException();
    }
}