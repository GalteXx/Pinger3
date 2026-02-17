using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pinger3.ViewModels.Design
{
    public class MainWindowViewModelDesign : ViewModelBase
    {
        public ObservableCollection<IPingViewModel> PingTargets { get; }

        public MainWindowViewModelDesign()
        {
            PingTargets = [];
            for (int i = 0; i < 4; i++)
            {
                PingTargets.Add(new PingViewModelDesign());
            }

        }
    }
}
