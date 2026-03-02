using Avalonia.Controls;
using Avalonia.Input;
using Pinger3.ViewModels.PageViewModels;

namespace Pinger3.Views;

public partial class PopoutWindow : Window
{
    public PopoutWindow()
    {
        InitializeComponent();
    }

    public PopoutWindow(IPopoutWindowViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
}