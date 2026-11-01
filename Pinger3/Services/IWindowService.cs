using Avalonia.Controls;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IWindowService
{
    void ShowWindow<T>() where T : Window;
    void ShowEndpointEditorWindow(EndpointModel endpointModel);
}