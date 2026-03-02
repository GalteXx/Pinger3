using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pinger3.ViewModels.PageViewModels
{
    public partial class PopoutWindowViewModel : INotifyPropertyChanged, IPopoutWindowViewModel
    {
        //TODO: BindStyles
        private double _transparency = 0.8;
        private bool _clickThrough = false;

        public bool ClickThrough
        {
            get => _clickThrough;
            set
            {
                _clickThrough = value;
                OnPropertyChanged();
            }
        }
        public double Transparency
        {
            get => _transparency;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _transparency = value;
                    OnPropertyChanged();
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException($"{value} is not in [0;1] range");
                }
            }
        }

        public ObservableCollection<IPingingTargetViewModel> PingViewModels { get; }

        public PopoutWindowViewModel()
        {
            PingViewModels = [];
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
