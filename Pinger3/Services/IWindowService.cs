using Avalonia.Controls;

namespace Pinger3.Services
{
    public interface IWindowService
    {
        public void ShowWindow<T>() where T : Window;
    }
}