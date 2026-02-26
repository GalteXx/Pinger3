using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Pinger3.Converters
{
    internal class TimeSpanToLargestUnitConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not TimeSpan span)
                return Avalonia.Data.BindingNotification.UnsetValue;
            string output = "";
            if (span.Days > 0)
                output += $"{span.Days}d ";
            if (span.Hours > 0)
                output += $"{span.Hours}h ";
            if (span.Minutes > 0)
                output += $"{span.Minutes}m ";
            if (span.Seconds > 0)
                output += $"{span.Seconds}s ";
            if (span.Milliseconds > 0)
                output += $"{span.Milliseconds}ms ";
            return string.IsNullOrWhiteSpace(output) ? "0ms" : output.TrimEnd();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
