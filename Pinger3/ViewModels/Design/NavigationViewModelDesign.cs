using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;

namespace Pinger3.ViewModels.Design
{
    internal class NavigationViewModelDesign : INavigationViewModel
    {
        public IRelayCommand OpenPopoutWindowCommand => new RelayCommand(() => { });

        public IRelayCommand OpenPopoutSettingsWindowCommand => new RelayCommand(() => { });
    }
}
