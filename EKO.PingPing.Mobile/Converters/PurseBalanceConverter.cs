using System.Globalization;

namespace EKO.PingPing.Mobile.Converters;

/// <summary>
/// Converter that will convert the balance of the purse to a string.
/// </summary>
internal sealed class PurseBalanceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double)
        {
            return null;
        }

        var balance = (double)value;

        // We want to show the balance in the format of € 0.00
        return $"€ {balance:F2}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
