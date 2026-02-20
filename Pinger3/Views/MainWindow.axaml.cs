using Avalonia.Controls;
using Avalonia.Interactivity;
using Pinger3.ViewModels;
using System;
using System.Threading.Tasks;

namespace Pinger3.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                await vm!.LoadPingingTargets();
        }
    }
}