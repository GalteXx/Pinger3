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

    public IAsyncRelayCommand<string> TogglePingingCommand { get; } =
        new AsyncRelayCommand<string>((_, _) => Task.CompletedTask);

    public IAsyncRelayCommand<string> TogglePopOutCommand { get; } =
        new AsyncRelayCommand<string>((_, _) => Task.CompletedTask);
}