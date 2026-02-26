using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Pinger3.Converters
{
    internal class BoolToToggleStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not bool boolValue || parameter is not string converterParam)
                return Avalonia.Data.BindingNotification.UnsetValue;
            var options = converterParam.Split('|');
            if (options.Length != 2)
                return Avalonia.Data.BindingNotification.UnsetValue;
            return boolValue ? options[0] : options[1];
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
