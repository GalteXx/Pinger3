using CommunityToolkit.Mvvm.Input;

namespace Pinger3.ViewModels.Controls
{
    public interface INavigationViewModel
    {
        public IRelayCommand OpenPopoutWindowCommand { get; }
        public IRelayCommand OpenPopoutSettingsWindowCommand { get; }
    }
}
