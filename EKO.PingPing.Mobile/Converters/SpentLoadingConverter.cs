using EKO.PingPing.Mobile.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKO.PingPing.Mobile.Converters;

/// <summary>
/// Converter that converts a balance to a color.
/// </summary>
internal sealed class SpentLoadingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string)
        {
            return null;
        }

        var balance = (string)value;

        // Green for deposit, red for withdraw, black for no action.
        if (balance == string.Empty)
        {
            return AppResources.Generic_Loading;
        }
        else
        {
            return balance;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
