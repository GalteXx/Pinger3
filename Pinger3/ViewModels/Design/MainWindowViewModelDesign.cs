using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;

namespace Pinger3.ViewModels.Design;

public class MainWindowViewModelDesign : IMainWindowViewModel
{
    public MainWindowViewModelDesign()
    {
        for (var i = 0; i < 4; i++) Endpoints.Add(new PingingTargetViewModelDesign());
    }

    public ObservableCollection<IPingingTargetViewModel> Endpoints { get; } = [];

    public IAsyncRelayCommand<string> TogglePingingCommand =>
        new AsyncRelayCommand<string>((_, _) => Task.CompletedTask);

    public IAsyncRelayCommand<string> TogglePopOutCommand =>
        new AsyncRelayCommand<string>((_, _) => Task.CompletedTask);

    public IAsyncRelayCommand CreateNewEndpointCommand =>
        new AsyncRelayCommand<string>((_, _) => Task.CompletedTask);

    public IRelayCommand<string> EditEndpointCommand =>
        new AsyncRelayCommand<string>((_, _) => Task.CompletedTask);
}