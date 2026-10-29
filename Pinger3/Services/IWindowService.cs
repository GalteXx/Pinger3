using Avalonia.Controls;

namespace Pinger3.Services
{
    public interface IWindowService
    {
        void ShowWindow<T>() where T : Window;
    }
}