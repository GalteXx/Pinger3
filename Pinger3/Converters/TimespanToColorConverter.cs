using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Pinger3.Converters
{
    internal class TimespanToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not TimeSpan timeSpan)
                throw new ArgumentException("Invalid value or parameter type");

            if (timeSpan == TimeSpan.FromMilliseconds(-1d))
                return targetType == typeof(Color) ? Colors.Gray
                    : new SolidColorBrush(Colors.Gray);
            //i set 100ms a time for the ping to get the "reddest", as I dont want to mess with tuples for now
            var clamValue = Math.Clamp(timeSpan.TotalMilliseconds / 100, 0, 1);

            byte r, g, b = 0;

            if (clamValue < 0.5)
            {
                double t = clamValue / 0.5;
                r = (byte)(255 * t);
                g = 255;
            }
            else
            {
                double t = (clamValue - 0.5) / 0.5;
                r = 255;
                g = (byte)(255 * (1 - t));
            }
            if (targetType == typeof(Color))
                return Color.FromRgb(r, g, b);
            else if (targetType == typeof(IBrush))
                return new SolidColorBrush(Color.FromRgb(r, g, b));
            else
                throw new ArgumentException("Invalid target type");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
