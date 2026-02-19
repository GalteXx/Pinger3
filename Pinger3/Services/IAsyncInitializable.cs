using System.Threading.Tasks;

namespace Pinger3.Services
{
    public interface IAsyncInitializable
    {
        public Task InitializeAsync();

    }
}
