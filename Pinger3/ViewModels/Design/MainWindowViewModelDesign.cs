using System.Collections.ObjectModel;

namespace Pinger3.ViewModels.Design
{
    public class MainWindowViewModelDesign : IMainWindowViewModel
    {
        public ObservableCollection<IPingViewModel> ValidPingingTargets { get; }

        public MainWindowViewModelDesign()
        {
            ValidPingingTargets = [];
            for (int i = 0; i < 4; i++)
            {
                ValidPingingTargets.Add(new PingViewModelDesign());
            }
        }
    }
}
