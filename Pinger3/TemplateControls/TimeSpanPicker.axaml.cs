using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Pinger3.TemplateControls;

public class TimeSpanPicker : TemplatedControl
{
    public static readonly StyledProperty<TimeSpan> ValueProperty =
        AvaloniaProperty.Register<TimeSpanPicker, TimeSpan>(
            nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<int> HoursProperty =
        AvaloniaProperty.Register<TimeSpanPicker, int>(nameof(Hours));

    public static readonly StyledProperty<int> MinutesProperty =
        AvaloniaProperty.Register<TimeSpanPicker, int>(nameof(Minutes));

    public static readonly StyledProperty<int> SecondsProperty =
        AvaloniaProperty.Register<TimeSpanPicker, int>(nameof(Seconds));

    public static readonly StyledProperty<int> MillisecondsProperty =
        AvaloniaProperty.Register<TimeSpanPicker, int>(nameof(Milliseconds));

    public TimeSpan Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public int Hours
    {
        get => GetValue(HoursProperty);
        set => SetValue(HoursProperty, value);
    }

    public int Minutes
    {
        get => GetValue(MinutesProperty);
        set => SetValue(MinutesProperty, value);
    }

    public int Seconds
    {
        get => GetValue(SecondsProperty);
        set => SetValue(SecondsProperty, value);
    }

    public int Milliseconds
    {
        get => GetValue(MillisecondsProperty);
        set => SetValue(MillisecondsProperty, value);
    }

    private bool _updating;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (_updating) return;

        if (change.Property == ValueProperty)
        {
            _updating = true;
            try
            {
                Hours = Value.Hours;
                Minutes = Value.Minutes;
                Seconds = Value.Seconds;
                Milliseconds = Value.Milliseconds;
            }
            finally
            {
                _updating = false;
            }

            return;
        }

        if (change.Property != HoursProperty && change.Property != MinutesProperty &&
            change.Property != SecondsProperty && change.Property != MillisecondsProperty)
            return;
        _updating = true;
        try
        {
            Value = new TimeSpan(0, Hours, Minutes, Seconds, Milliseconds);
        }
        finally
        {
            _updating = false;
        }
    }
}