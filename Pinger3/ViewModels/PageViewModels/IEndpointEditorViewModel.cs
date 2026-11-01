using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;

namespace Pinger3.ViewModels.PageViewModels;

public interface IEndpointEditorViewModel : INotifyPropertyChanged
{
    public string EndpointId { get; }
    public string EndpointName { get; set; }
    public string EndpointAddress { get; set; }
    public string EndpointRequestDelay { get; set; }
    public IAsyncRelayCommand EditEndpointCommand { get; }
    public IAsyncRelayCommand DeleteEndpointCommand { get; }
    void SetEndpointModel(EndpointModel model);
}