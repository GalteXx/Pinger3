using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;

namespace Pinger3.ViewModels.PageViewModels;

public interface IEndpointEditorViewModel : INotifyPropertyChanged
{
    string EndpointId { get; }
    string EndpointName { get; set; }
    string EndpointAddress { get; set; }
    string EndpointRequestDelay { get; set; }
    IAsyncRelayCommand EditEndpointCommand { get; }
    IAsyncRelayCommand DeleteEndpointCommand { get; }
    void SetEndpointModel(EndpointModel model);

    event EventHandler? OperationFinished;
}