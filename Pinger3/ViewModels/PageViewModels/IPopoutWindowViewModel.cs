using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public interface IPopoutWindowViewModel : INotifyPropertyChanged
    {
        string SelectedStyleClassName { get; set; }
        bool ClickThrough { get; set; }
        ObservableCollection<IPingingTargetViewModel> PingViewModels { get; }
        double Opacity { get; set; }
    }
}