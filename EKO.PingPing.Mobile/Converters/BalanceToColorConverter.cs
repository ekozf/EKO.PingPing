using System.Globalization;

namespace EKO.PingPing.Mobile.Converters;

/// <summary>
/// Converter that converts a balance to a color.
/// </summary>
internal sealed class BalanceToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double)
        {
            return null;
        }

        var balance = (double)value;

        // Green for deposit, red for withdraw, black for no action.
        return balance switch
        {
            < 0 => Color.FromArgb("#ff6962"),
            > 0 => Color.FromArgb("#76db76"),
            _ => Color.FromArgb("#000000")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
