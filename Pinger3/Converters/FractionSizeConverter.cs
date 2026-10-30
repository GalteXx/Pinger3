using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Pinger3.Converters;

internal class FractionSizeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double size || parameter is not string converterParam ||
            !double.TryParse(converterParam, NumberStyles.Any, CultureInfo.InvariantCulture, out var fraction))
            return BindingNotification.UnsetValue;
        return size / fraction;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}