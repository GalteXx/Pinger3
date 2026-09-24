using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class SettingsViewModel(IPopoutWindowViewModel popoutVm)
        : ObservableObject, ISettingsViewModel, INotifyPropertyChanged
    {
        [ObservableProperty] private IPopoutWindowViewModel _popoutVM = popoutVm;
    }
}