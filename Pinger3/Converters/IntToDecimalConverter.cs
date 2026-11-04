using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Pinger3.TemplateControls;

public class IntToDecimalConverter : IValueConverter
{
    public static readonly IntToDecimalConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is int i ? i : 0m;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is decimal d ? (int)d : BindingOperations.DoNothing;
    }
}