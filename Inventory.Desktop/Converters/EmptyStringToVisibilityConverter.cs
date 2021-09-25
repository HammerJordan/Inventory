using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Inventory.Desktop.Converters
{
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string v)
                return null;


            if (bool.TryParse(parameter as string, out bool b))
                return string.IsNullOrEmpty(v) ^ b ? Visibility.Visible : Visibility.Collapsed;

            return string.IsNullOrEmpty(v) ? Visibility.Visible : Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}