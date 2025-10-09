using Avalonia.Data.Converters;   // ← これ
using System;
using System.Globalization;

namespace TodoApp.Converters
{
    /// <summary>IsCompleted(true) → 0.6 / false → 1.0</summary>
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b && b ? 0.6 : 1.0;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => false;
    }
}
