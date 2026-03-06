using Avalonia.Controls;

namespace Pinger3.Services
{
    public interface IWindowService
    {
        T GetWindowInstance<T>() where T : Window;
        void ShowWindow<T>() where T : Window;
    }
}