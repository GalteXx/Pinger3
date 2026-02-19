using Avalonia.Controls;

namespace Pinger3.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = DataContext as ViewModels.MainWindowViewModel;
            //Loaded += async (_, __) => await vm!.InitializeAsync();
        }
    }
}