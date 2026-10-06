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

            if (timeSpan < TimeSpan.Zero)
                return targetType == typeof(Color) ? Colors.Gray
                    : new SolidColorBrush(Colors.Gray);
            
            // TODO: Add setting for color timings
            
            var color = TimeSpanToColor(timeSpan);

            if (targetType == typeof(Color))
                return color;
            
            if (targetType == typeof(IBrush))
                return new SolidColorBrush(color);
            
            throw new ArgumentException("Invalid target type");
        }

        private static Color TimeSpanToColor(TimeSpan timeSpan)
        {
            var clamValue = Math.Clamp(timeSpan.TotalMilliseconds / 100, 0, 1);

            byte r, g, b = 0;

            if (clamValue < 0.5)
            {
                var t = clamValue / 0.5;
                r = (byte)(255 * t);
                g = 255;
            }
            else
            {
                var t = (clamValue - 0.5) / 0.5;
                r = 255;
                g = (byte)(255 * (1 - t));
            }

            return Color.FromRgb(r, g, b);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
