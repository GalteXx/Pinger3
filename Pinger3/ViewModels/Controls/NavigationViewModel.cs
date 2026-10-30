using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using Pinger3.Services;
using Pinger3.Views;

namespace Pinger3.ViewModels.Controls;

public partial class NavigationViewModel(IWindowService windowService) : INavigationViewModel, INotifyPropertyChanged
{
    private readonly IWindowService _windowService = windowService;

    public event PropertyChangedEventHandler? PropertyChanged;

    [RelayCommand]
    private void OpenPopoutWindow()
    {
        _windowService.ShowWindow<PopoutWindow>();
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}