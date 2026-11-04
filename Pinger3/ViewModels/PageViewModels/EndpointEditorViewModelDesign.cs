using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.Design;

namespace Pinger3.ViewModels.PageViewModels;

public class EndpointEditorViewModelDesign : IEndpointEditorViewModel
{
    private readonly IPingingTargetViewModel _viewModel = new PingingTargetViewModelDesign();
    public event PropertyChangedEventHandler? PropertyChanged;
    public string EndpointId => _viewModel.Id;

    public string EndpointName
    {
        get => _viewModel.Name;
        set { }
    }

    public string EndpointAddress
    {
        get => _viewModel.Address;
        set { }
    }

    public TimeSpan EndpointRequestDelay
    {
        get => _viewModel.DelayBetweenRequests;
        set { }
    }

    public IAsyncRelayCommand EditEndpointCommand 
        => new AsyncRelayCommand(_ => Task.CompletedTask);
    public IAsyncRelayCommand DeleteEndpointCommand 
        => new AsyncRelayCommand(_ => Task.CompletedTask);

    public void SetEndpointModel(EndpointModel model)
    {
        throw new NotImplementedException();
    }

    public event EventHandler? OperationFinished;
}