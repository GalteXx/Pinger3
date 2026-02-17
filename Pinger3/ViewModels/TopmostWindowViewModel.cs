using Pinger3.Models;
using Pinger3.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Pinger3.ViewModels
{
    internal class TopmostWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PingViewModel> PingViewModels { get; }

        public TopmostWindowViewModel(ObservableCollection<PingTargetModel> pingModels, ResponeAwaitingTimeUpdaterService timeUpdater)
        {
            PingViewModels = new(pingModels.Select(x => new PingViewModel(x, timeUpdater)));
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
