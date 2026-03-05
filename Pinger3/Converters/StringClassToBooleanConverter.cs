using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Pinger3.Converters
{
    internal class StringClassToBooleanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string targetClass || parameter is not string compareClassName)
                return Avalonia.Data.BindingNotification.UnsetValue;
            return targetClass == compareClassName;
            //wow, class binding is the worst thing i've ever coded
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
