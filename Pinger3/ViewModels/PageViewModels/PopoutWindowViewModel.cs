using System;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pinger3.ViewModels.PageViewModels;

public partial class PopoutWindowViewModel : ObservableObject, IPopoutWindowViewModel
{
    //TODO: BindStyles
    [ObservableProperty] private bool _clickThrough;
    [ObservableProperty] private string _selectedStyleClassName = "CompactResponseTime";
    [ObservableProperty] private double _opacity = 1;

    public ObservableCollection<IPingingTargetViewModel> PingViewModels { get; } = [];

    partial void OnOpacityChanging(double value)
    {
        if (value is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not in [0;1] range");
    }



}