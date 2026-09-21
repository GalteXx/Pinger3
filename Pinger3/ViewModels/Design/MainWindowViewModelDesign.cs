using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.Design
{
    public class MainWindowViewModelDesign : IMainWindowViewModel
    {
        public ObservableCollection<IPingingTargetViewModel> Endpoints { get; } = [];
        public IRelayCommand<string> TogglePingingCommand { get; }
        public MainWindowViewModelDesign(IRelayCommand<string> togglePingingCommand)
        {
            TogglePingingCommand = togglePingingCommand;
            for (int i = 0; i < 4; i++)
            {
                Endpoints.Add(new PingingTargetViewModelDesign());
            }
        }
    }
}
