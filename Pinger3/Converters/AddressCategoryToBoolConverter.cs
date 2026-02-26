using Avalonia.Data.Converters;
using Pinger3.ViewModels.PageViewModels;
using System;
using System.Globalization;

namespace Pinger3.Converters
{
    internal class AddressCategoryToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is not PingingTargetCategory category)
                return Avalonia.Data.BindingNotification.UnsetValue;
            return category != PingingTargetCategory.InvalidTargets;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
