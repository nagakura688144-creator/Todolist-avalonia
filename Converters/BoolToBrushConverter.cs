using Avalonia.Data.Converters;   // ← これ
using Avalonia.Media;
using System;
using System.Globalization;

namespace TodoApp.Converters
{
    /// <summary>IsOverdue(true) → 赤背景 / false → 薄いグレー背景</summary>
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var isOverdue = value is bool b && b;
            return isOverdue
                ? new SolidColorBrush(Colors.Red)
                : new SolidColorBrush(Color.Parse("#00000014"));
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => false;
    }
}
