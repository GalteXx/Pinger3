using Pinger3.Models;
using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Pinger3.ViewModels.WindowViewModels
{
    internal class TopmostWindowViewModel : INotifyPropertyChanged //rename to Popout
    {
        public ObservableCollection<PingingTargetViewModel> PingViewModels { get; }

        public TopmostWindowViewModel(ObservableCollection<PingTargetModel> pingModels, ResponeAwaitingTimeUpdaterService timeUpdater)
        {
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
