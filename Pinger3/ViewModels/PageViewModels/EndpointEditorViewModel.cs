using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using Pinger3.Services;

namespace Pinger3.ViewModels.PageViewModels;

public partial class EndpointEditorViewModel(IEndpointRepository endpointRepository)
    : ObservableObject, IEndpointEditorViewModel
{
    private string _endpointId = string.Empty;

    public string EndpointId
    {
        get => _endpointId;
        private set => SetProperty(ref _endpointId, value);
    }

    [ObservableProperty] private string _endpointName = string.Empty;

    [ObservableProperty] private string _endpointAddress = string.Empty;

    [ObservableProperty] private string _endpointRequestDelay = string.Empty;

    public void SetEndpointModel(EndpointModel model)
    {
        EndpointId = model.Id;
        EndpointName = model.Name;
        EndpointAddress = model.Address;
        EndpointRequestDelay = model.DelayBetweenRequests.ToString();
    }

    [RelayCommand]
    private async Task EditEndpoint(CancellationToken ct)
    {
        if (!TimeSpan.TryParse(EndpointRequestDelay, out var delay))
            return;

        var model = new EndpointModel(EndpointId, EndpointName, EndpointAddress, delay);
        await endpointRepository.EditEndpointAsync(model, ct);
        OnOperationFinished();
    }

    [RelayCommand]
    private async Task DeleteEndpoint(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(EndpointId))
            return;
        await endpointRepository.DeleteEndpointAsync(EndpointId, ct);
        OnOperationFinished();
    }

    public event EventHandler? OperationFinished;

    protected void OnOperationFinished()
    {
        OperationFinished?.Invoke(this, EventArgs.Empty);
    }
}