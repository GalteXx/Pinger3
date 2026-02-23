using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Pinger3.Converters
{
    internal class FractionSizeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not double size || parameter is not string converterParam || 
                !double.TryParse(converterParam, NumberStyles.Any, CultureInfo.InvariantCulture, out double fraction))
                return Avalonia.Data.BindingNotification.UnsetValue;
            return size / fraction;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
