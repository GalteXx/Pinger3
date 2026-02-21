using Pinger3.Services;
using Pinger3.ViewModels.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pinger3.ViewModels.WindowViewModels
{
    internal class PopoutWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PingingTargetViewModel> PingViewModels { get; }

        public PopoutWindowViewModel(IEnumerable<IPingingTargetViewModel> pingModels, ResponeAwaitingTimeUpdaterService timeUpdater)
        {
            ;
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
