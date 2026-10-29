using Avalonia.Controls;
using Avalonia.Interactivity;
using Pinger3.ViewModels.PageViewModels;
using System;
using System.Collections.Generic;

namespace Pinger3.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        public MainWindow(IMainWindowViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }


        private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                 vm.OnMainWindowLoaded();
        }

        private void NativeMenuItem_Click(object? sender, System.EventArgs e)
        {
        }
    }
}