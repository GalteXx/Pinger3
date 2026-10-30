using System.Collections.ObjectModel;
using System.ComponentModel;
using Pinger3.ViewModels.Controls;

namespace Pinger3.ViewModels.PageViewModels;

public interface IPopoutWindowViewModel : INotifyPropertyChanged
{
    string SelectedStyleClassName { get; set; }
    bool ClickThrough { get; set; }
    ObservableCollection<IPopOutEndpointViewModel> PingViewModels { get; }
    double Opacity { get; set; }
}