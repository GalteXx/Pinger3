using System.Threading.Tasks;

namespace Pinger3.Services.DependencyInjection
{
    public class AsyncIntializer
    {
        public static async Task InitializeAsync(IAsyncInitializable component)
        {
            await component.InitializeAsync();
        }
    }
}
