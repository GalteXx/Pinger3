using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Pinger3.Converters
{
    internal class TimespanToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not TimeSpan timeSpan)
                throw new ArgumentException("Invalid value type");
            if (timeSpan == TimeSpan.MinValue || timeSpan == TimeSpan.FromMilliseconds(-1d))
                return "N/A";
            if (timeSpan == TimeSpan.MaxValue)
                return "Timeout";
            return $"{double.Round(timeSpan.TotalMilliseconds, 1)} ms";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
