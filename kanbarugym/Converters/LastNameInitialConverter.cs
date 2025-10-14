using System.Globalization;
using Microsoft.Maui.Controls;

namespace kanbarugym.Converters;

public class LastNameInitialConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var name = value as string;
        if (string.IsNullOrWhiteSpace(name)) return string.Empty;
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length >= 2)
        {
            // Try to use the first surname (commonly the second token)
            var apellido = parts[1];
            return apellido[0].ToString().ToUpper(culture);
        }
        // Fallback: first letter of the only token
        return parts[0][0].ToString().ToUpper(culture);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
