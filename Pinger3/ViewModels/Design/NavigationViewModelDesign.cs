using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;

namespace Pinger3.ViewModels.Design;

internal class NavigationViewModelDesign : INavigationViewModel
{
    public IRelayCommand OpenPopoutSettingsWindowCommand => new RelayCommand(() => { });
    public IRelayCommand OpenPopoutWindowCommand => new RelayCommand(() => { });
}