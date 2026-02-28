using CommunityToolkit.Mvvm.Input;
using Pinger3.Services;
using Pinger3.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pinger3.ViewModels.Controls
{
    public partial class NavigationViewModel(IWindowService windowService) : INavigationViewModel, INotifyPropertyChanged
    {
        private readonly IWindowService _windowService = windowService;

        [RelayCommand]
        private void OpenPopoutWindow()
        {
            _windowService.ShowWindow<PopoutWindow>();
        }
        [RelayCommand]
        private void OpenPopoutSettingsWindow()
        {
            _windowService.ShowWindow<SettingsWindow>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
