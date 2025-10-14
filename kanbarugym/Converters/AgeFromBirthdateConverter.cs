using System.Globalization;
using Microsoft.Maui.Controls;

namespace kanbarugym.Converters;

public class AgeFromBirthdateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return null;
        var s = value.ToString();
        if (string.IsNullOrWhiteSpace(s)) return null;

        if (!DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var birth))
        {
            // Try current culture as fallback
            if (!DateTime.TryParse(s, culture, DateTimeStyles.AssumeLocal, out birth))
                return null;
        }

        var today = DateTime.Today;
        var age = today.Year - birth.Year;
        if (birth.Date > today.AddYears(-age)) age--;
        if (age < 0) age = 0;
        return age;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
