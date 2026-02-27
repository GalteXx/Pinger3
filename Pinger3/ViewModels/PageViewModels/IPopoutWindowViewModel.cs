using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public interface IPopoutWindowViewModel
    {
        bool ClickThrough { get; set; }
        ObservableCollection<IPingingTargetViewModel> PingViewModels { get; }
        double Transparency { get; set; }
    }
}