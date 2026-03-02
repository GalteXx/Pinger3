using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pinger3.ViewModels.Design
{
    internal class PopoutWindowViewModelDesign : IPopoutWindowViewModel
    {
        private readonly List<IPingingTargetViewModel> _pingTargets;
        public PopoutWindowViewModelDesign()
        {
            _pingTargets = [];
             for (int i = 0; i < 4; i++)
             {
                 _pingTargets.Add(new PingingTargetViewModelDesign());
            }
        }

        public bool ClickThrough { get => false; set { } }

        public ObservableCollection<IPingingTargetViewModel> PingViewModels =>
        [
            new PingingTargetViewModelDesign(),
            new PingingTargetViewModelDesign(),
            new PingingTargetViewModelDesign(),
            new PingingTargetViewModelDesign()
        ];

        public double Opacity { get => 0; set { } }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
