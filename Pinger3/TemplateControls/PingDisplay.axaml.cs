using Avalonia;
using Avalonia.Controls.Primitives;
using System;

namespace Pinger3.TemplateControls;

public class PingDisplay : TemplatedControl
{
    public static readonly StyledProperty<TimeSpan> PingProperty =
        AvaloniaProperty.Register<PingDisplay, TimeSpan>(nameof(Ping));
    public static readonly StyledProperty<TimeSpan> ResponseWaitingTimeProperty =
        AvaloniaProperty.Register<PingDisplay, TimeSpan>(nameof(ResponseWaitingTime));
    public static readonly StyledProperty<string> AddressProperty =
        AvaloniaProperty.Register<PingDisplay, string>(nameof(Address));
    public static readonly StyledProperty<string> AddressNameProperty =
        AvaloniaProperty.Register<PingDisplay, string>(nameof(AddressName));

    public TimeSpan Ping
    {
        get => GetValue(PingProperty);
        set => SetValue(PingProperty, value);
    }

    public TimeSpan ResponseWaitingTime
    {
        get => GetValue(ResponseWaitingTimeProperty);
        set => SetValue(ResponseWaitingTimeProperty, value);
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