using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Pinger3.Converters;

internal class BoolToToggleStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool boolValue || parameter is not string converterParam)
            return BindingNotification.UnsetValue;
        var options = converterParam.Split('|');
        if (options.Length != 2)
            return BindingNotification.UnsetValue;
        return boolValue ? options[0] : options[1];
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}