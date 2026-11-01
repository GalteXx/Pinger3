using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;

namespace Pinger3.ViewModels.PageViewModels;

public interface IMainWindowViewModel
{
    public ObservableCollection<IPingingTargetViewModel> Endpoints { get; }

    public IAsyncRelayCommand<string> TogglePingingCommand { get; }

    public IAsyncRelayCommand<string> TogglePopOutCommand { get; }

    public IAsyncRelayCommand CreateNewEndpointCommand { get; }
    
    public IRelayCommand<string> EditEndpointCommand { get; }
}