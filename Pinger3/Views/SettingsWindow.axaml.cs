using Avalonia.Controls;
using Pinger3.ViewModels.PageViewModels;

namespace Pinger3;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }

    public SettingsWindow(ISettingsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}