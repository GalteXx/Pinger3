using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Avalonia.Threading;
using Pinger3.Views;

namespace Pinger3.Services;

public sealed class TrayIconService : IDisposable
{
    private readonly TrayIcon _trayIcon;
    private readonly IWindowService _windowService;

    public TrayIconService(IWindowService windowService)
    {
        _windowService = windowService;

        _trayIcon = new TrayIcon
        {
            ToolTipText = "Pinger",
            Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://Pinger3/Assets/icon.png")))
        };

        InitializeMenu();
        _trayIcon.Clicked += OnTrayIconClicked;
    }

    public void Dispose()
    {
        _trayIcon.Dispose();
    }

    private void InitializeMenu()
    {
        var menu = new NativeMenu();

        var showItem = new NativeMenuItem("Main Window");
        showItem.Click += (s, e) => ShowMainWindow();


        var exitItem = new NativeMenuItem("Stop");
        exitItem.Click += (s, e) => ExitApplication();

        menu.Items.Add(showItem);
        menu.Items.Add(new NativeMenuItemSeparator());
        menu.Items.Add(exitItem);

        _trayIcon.Menu = menu;
    }

    private void ShowMainWindow()
    {
        _windowService.ShowWindow<MainWindow>();
    }

    private void ExitApplication()
    {
        Dispose();
        ((IClassicDesktopStyleApplicationLifetime?)Application.Current?.ApplicationLifetime)?.Shutdown();
    }

    private void OnTrayIconClicked(object? sender, EventArgs e)
    {
        _windowService.TogglePopOutWidgedVisibility();
    }

    public void Show()
    {
        _trayIcon.IsVisible = true;
    }
}