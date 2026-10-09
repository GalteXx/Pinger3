using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IStorageInitializer
{
    Task Initialize();
}