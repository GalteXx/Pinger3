using Avalonia.Controls;
using Pinger3.ViewModels.PageViewModels;
using System;

namespace Pinger3;

public partial class PopoutSettingsWindow : Window
{
    public PopoutSettingsWindow()
    {
        InitializeComponent();
    }

    public PopoutSettingsWindow(IPopoutWindowViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}