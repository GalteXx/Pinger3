using Avalonia.Controls;
using Avalonia.Interactivity;
using Pinger3.ViewModels.PageViewModels;

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
                await vm!.OnMainWindowLoaded();
        }

        private void NativeMenuItem_Click(object? sender, System.EventArgs e)
        {
        }
    }
}