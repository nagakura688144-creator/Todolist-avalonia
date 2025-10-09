using Avalonia.Data.Converters;
using Avalonia.Styling;
using System;
using System.Globalization;

namespace TodoApp.Converters
{
    /// <summary>bool → ThemeVariant (true: Dark / false: Light)</summary>
    public class BoolToThemeVariantConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => (value is bool b && b) ? ThemeVariant.Dark : ThemeVariant.Light;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is ThemeVariant v && v == ThemeVariant.Dark;
    }
}
