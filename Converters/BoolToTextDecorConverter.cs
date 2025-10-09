using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace TodoApp.Converters
{
    /// <summary>IsCompleted(true) → 取り消し線 / false → なし</summary>
    public class BoolToTextDecorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b && b ? TextDecorations.Strikethrough : new TextDecorationCollection();

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => false;
    }
}
