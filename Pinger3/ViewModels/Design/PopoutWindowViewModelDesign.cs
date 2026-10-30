using System.Collections.ObjectModel;
using System.ComponentModel;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;

namespace Pinger3.ViewModels.Design;

internal class PopoutWindowViewModelDesign : IPopoutWindowViewModel
{
    public bool ClickThrough
    {
        get => false;
        set { }
    }

    public ObservableCollection<IPopOutEndpointViewModel> PingViewModels =>
    [
        new PingingTargetViewModelDesign(),
        new PingingTargetViewModelDesign(),
        new PingingTargetViewModelDesign(),
        new PingingTargetViewModelDesign()
    ];

    public double Opacity
    {
        get => 0;
        set { }
    }

    public string SelectedStyleClassName
    {
        get => "Full";
        set { }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}