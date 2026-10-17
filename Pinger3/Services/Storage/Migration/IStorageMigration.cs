using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Pinger3.Services.Migration;

public interface IStorageMigration
{
    int FromVersion { get; }
    int ToVersion { get; }

    Task Migrate(SqliteConnection connection);
}