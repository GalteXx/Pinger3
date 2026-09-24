using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Pinger3.ViewModels.Design
{
    public class MainWindowViewModelDesign : IMainWindowViewModel
    {
        public ObservableCollection<IPingingTargetViewModel> Endpoints { get; } = [];

        public IAsyncRelayCommand<string> TogglePingingCommand { get; } = new AsyncRelayCommand<string>((_, _) => Task.CompletedTask);

        public MainWindowViewModelDesign()
        {
            for (var i = 0; i < 4; i++)
            {
                Endpoints.Add(new PingingTargetViewModelDesign());
            }
        }
    }
}