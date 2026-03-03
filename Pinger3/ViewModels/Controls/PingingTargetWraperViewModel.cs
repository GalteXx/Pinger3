using Pinger3.ViewModels.PageViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pinger3.ViewModels.Controls
{
    public partial class PingingTargetWrapperViewModel : INotifyPropertyChanged, IPingingTargetWrapperViewModel
    {
        private readonly IPingingTargetViewModel _targetVM;
        private readonly IPopoutWindowViewModel _popoutVM;


        public IPingingTargetViewModel TargetViewModel => _targetVM;

        public bool IsMarkedForPopout
        {
            get => _popoutVM.PingViewModels.Contains(_targetVM);
            set
            {
                if (value)
                    _popoutVM.PingViewModels.Add(_targetVM);
                else
                    _popoutVM.PingViewModels.Remove(_targetVM);
            }

        }

        public PingingTargetWrapperViewModel(IPingingTargetViewModel targetVM, IPopoutWindowViewModel popoutVm)
        {
            _targetVM = targetVM;
            _popoutVM = popoutVm;
            _targetVM.PropertyChanged += (sender, e) => { PropertyChanged?.Invoke(sender, e); };
        }

        
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
