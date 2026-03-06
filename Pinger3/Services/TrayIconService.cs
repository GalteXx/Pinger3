using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Pinger3.Views;
using System;

namespace Pinger3.Services
{
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
                Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://Pinger3/Assets/icon.ico")))
            };

            InitializeMenu();
            _trayIcon.Clicked += OnTrayIconClicked;
        }

        private void InitializeMenu()
        {
            var menu = new NativeMenu();

            var showItem = new NativeMenuItem("Main Window");
            showItem.Click += (s, e) => ShowMainWindow();

            var settingsItem = new NativeMenuItem("Settings");
            settingsItem.Click += (s, e) => ShowSettings();

            var exitItem = new NativeMenuItem("Stop");
            exitItem.Click += (s, e) => ExitApplication();

            menu.Items.Add(showItem);
            menu.Items.Add(settingsItem);
            menu.Items.Add(new NativeMenuItemSeparator());
            menu.Items.Add(exitItem);

            _trayIcon.Menu = menu;
        }
        private void ShowMainWindow()
        {
            _windowService.ShowWindow<MainWindow>();
        }

        private void ShowSettings()
        {
            _windowService.ShowWindow<SettingsWindow>();
        }

        private void ExitApplication()
        {
            _trayIcon.IsVisible = false;
            ((IClassicDesktopStyleApplicationLifetime?)Application.Current?.ApplicationLifetime)?.Shutdown();
        }

        private void OnTrayIconClicked(object? sender, EventArgs e)
        {
            var popout = _windowService.GetWindowInstance<PopoutWindow>();
            popout.IsVisible = !popout.IsVisible;
        }

        public void Show()
        {
            _trayIcon.IsVisible = true;
        }

        public void Dispose()
        {
            _trayIcon?.Dispose();
        }
    }
}
