using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Utilities;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMoneyUi;
public class ToColorConverter : IValueConverter
{
    /// <inheritdoc/>
    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (value is Color valueColor)
        {
            return valueColor;
        }
        else if (value is HslColor valueHslColor)
        {
            return valueHslColor.ToRgb();
        }
        else if (value is HsvColor valueHsvColor)
        {
            return valueHsvColor.ToRgb();
        }
        else if (value is SolidColorBrush valueBrush)
        {
            // A brush may have an opacity set along with alpha transparency
            double alpha = valueBrush.Color.A * valueBrush.Opacity;

            return new Color(
                (byte)MathUtilities.Clamp(alpha, 0x00, 0xFF),
                valueBrush.Color.R,
                valueBrush.Color.G,
                valueBrush.Color.B);
        }
        else if (value is string s)
        {
            if(string.IsNullOrWhiteSpace(s)) return AvaloniaProperty.UnsetValue;
            return Color.Parse(s);
        }

        return AvaloniaProperty.UnsetValue;
    }

    /// <inheritdoc/>
    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        return value?.ToString();
    }
}