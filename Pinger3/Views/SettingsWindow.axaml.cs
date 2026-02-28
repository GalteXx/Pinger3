using Avalonia.Controls;
using Pinger3.ViewModels.PageViewModels;
using System;

namespace Pinger3;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }

    public SettingsWindow(IPopoutWindowViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}