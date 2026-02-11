using Avalonia;
using Avalonia.Controls.Primitives;

namespace Pinger3.TemplateControls;

public class PingDisplay : TemplatedControl
{
    public static readonly StyledProperty<string> PingProperty =
        AvaloniaProperty.Register<PingDisplay, string>(nameof(PingProperty));
    public static readonly StyledProperty<string> ResponsWaitingTimeProperty =
        AvaloniaProperty.Register<PingDisplay, string>(nameof(ResponsWaitingTimeProperty));
    public static readonly StyledProperty<string> AddressProperty =
        AvaloniaProperty.Register<PingDisplay, string>(nameof(AddressProperty));
    public static readonly StyledProperty<string> AddressNameProperty =
        AvaloniaProperty.Register<PingDisplay, string>(nameof(AddressNameProperty));

    public string Ping
    {
        get => GetValue(PingProperty);
        set => SetValue(PingProperty, value);
    }
    public string ResponsWaitingTime
    {
        get => GetValue(ResponsWaitingTimeProperty);
        set => SetValue(ResponsWaitingTimeProperty, value);
    }
    public string Address
    {
        get => GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }
    public string AddressName
    {
        get => GetValue(AddressNameProperty);
        set => SetValue(AddressNameProperty, value);
    }

}