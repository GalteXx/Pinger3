using Avalonia.Controls;
using Avalonia.Interactivity;
using Pinger3.ViewModels.PageViewModels;
using System;
using System.Collections.Generic;

namespace Pinger3.Views
{
    public partial class MainWindow : Window
    {
        public List<PingingTargetCategory> AddressCategories { get; } //this is technically View Logic, so no VM involved
        public MainWindow()
        {
            AddressCategories = [.. Enum.GetValues<PingingTargetCategory>()];
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                await vm!.OnMainWindowLoaded();
        }

        private void NativeMenuItem_Click(object? sender, System.EventArgs e)
        {
        }
    }
}