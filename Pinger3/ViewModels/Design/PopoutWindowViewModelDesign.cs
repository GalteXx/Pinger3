using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public bool ClickThrough { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public ObservableCollection<IPingingTargetViewModel> PingViewModels => throw new System.NotImplementedException();

        public double Transparency { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
