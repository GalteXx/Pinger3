using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Pinger3.Services.Migration;

public interface IStorageMigration
{
    string? FromVersion { get; }
    string ToVersion { get; }

    Task Migrate(SqliteConnection connection);
}