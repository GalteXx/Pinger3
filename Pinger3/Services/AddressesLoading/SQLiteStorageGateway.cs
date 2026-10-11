using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Pinger3.Models;

namespace Pinger3.Services;

public class SqliteStorageGateway(SqliteConnectionFactory connectionFactory)
    : IStorageGateway, IActiveEndpointsStorage, IPopOutEndpointsStorage
{
    public async IAsyncEnumerable<EndpointDto> ReadAllEndpointsAsync()
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

    public async Task<EndpointDto?> ReadEndpoint(string id)
    {
        var rows = await QueryAsync(
            "SELECT id, name, address, delay FROM endpoint WHERE id = @id",
            reader => new EndpointDto
            (
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3)
            ),
            new SqliteParameter("@id", id));

        return rows.FirstOrDefault();
    }

    public async Task EditEndpointAsync(EndpointDto dto) =>
        await ExecuteNonQueryCommandAsync(
            "UPDATE endpoint SET name = @name, address = @address, delay = @delay WHERE id = @id",
            new SqliteParameter("@id", dto.Id),
            new SqliteParameter("@name", dto.Name),
            new SqliteParameter("@address", dto.Name),
            new SqliteParameter("@delay", dto.RequestDelay));

    public async Task AddEndpointAsync(EndpointDto dto) =>
        await ExecuteNonQueryCommandAsync(
            "INSERT INTO endpoint (name, address, delay) VALUES (@name, @address, @delay)",
            new SqliteParameter("@id", dto.Id),
            new SqliteParameter("@name", dto.Name),
            new SqliteParameter("@address", dto.Name),
            new SqliteParameter("@delay", dto.RequestDelay));

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

    private async Task<T> ExecuteAsync<T>(string commandText, Func<SqliteCommand, Task<T>> execute,
        params SqliteParameter[] parameters)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = commandText;

        if (parameters.Length > 0)
            command.Parameters.AddRange(parameters);

        return await execute(command);
    }

    private Task<List<T>> QueryAsync<T>(
        string sql,
        Func<SqliteDataReader, T> map,
        params SqliteParameter[] parameters) =>
        ExecuteAsync(sql, async cmd =>
        {
            await using var reader = await cmd.ExecuteReaderAsync();
            var results = new List<T>();
            while (await reader.ReadAsync())
                results.Add(map(reader));
            return results;
        }, parameters);

    private Task<int> ExecuteNonQueryCommandAsync(string commandText, params SqliteParameter[] parameters) =>
        ExecuteAsync(commandText, cmd => cmd.ExecuteNonQueryAsync(), parameters);
}