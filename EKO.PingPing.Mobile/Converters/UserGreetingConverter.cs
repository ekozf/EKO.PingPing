using System.Globalization;

namespace EKO.PingPing.Mobile.Converters;

/// <summary>
/// Converter that will convert the user's name to a greeting.
/// </summary>
internal sealed class UserGreetingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var name = value as string;

        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var correctName = name.Trim().Split(' ').Last();

        return $"Hallo, {correctName}!";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
