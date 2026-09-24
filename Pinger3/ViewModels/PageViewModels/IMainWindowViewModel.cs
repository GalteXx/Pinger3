using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public interface IMainWindowViewModel
    {
        public ObservableCollection<IPingingTargetViewModel> Endpoints { get; }

        public IAsyncRelayCommand<string> TogglePingingCommand { get; }
    }
}