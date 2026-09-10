using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pinger3.Services;

internal interface IAddressesStorageLoader<T>
{
    Task<T> LoadStorageAsync();
}