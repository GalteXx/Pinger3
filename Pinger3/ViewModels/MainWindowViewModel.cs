namespace Pinger3.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public IPingViewModel PingViewModel { get; } = new PingViewModelDesign();
    }
}
