using Avalonia.Controls;
using Pinger3.TemplateControls;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System;
using System.Collections.Generic;

namespace Pinger3;

public partial class SettingsWindow : Window
{
    public static List<PingDisplayClass> DisplayableClasses => [.. Enum.GetValues<PingDisplayClass>()];
    public SettingsWindow()
    {
        InitializeComponent();
    }

    public SettingsWindow(ISettingsViewModel vm)
    {
        var dis = new PingDisplay();
        var cl = dis.Classes.GetEnumerator();
        InitializeComponent();
        DataContext = vm;
    }
}