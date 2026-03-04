using Avalonia.Data.Converters;
using Pinger3.Models;
using System;
using System.Globalization;

namespace Pinger3.Converters
{
    internal class ValidationErrorsToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is not ConfigValidationErrors errors)
                return Avalonia.Data.BindingNotification.UnsetValue;
            return (errors != ConfigValidationErrors.None) != ((parameter is null) || ((string)parameter == "inverted"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
