using System.Globalization;

namespace EKO.PingPing.Infrastructure.Helpers;

public static partial class PageParser
{
    /// <summary>
    /// Gets the balance from the page.
    /// </summary>
    /// <param name="page">Page to scrape</param>
    /// <returns>Scraped user balance</returns>
    private static double ParseBalance(string page)
    {
        var matches = BalanceRegex().Matches(page);

        if (matches.Count == 0)
            return 0;

        // Remove the currency symbol from the first matched balance
        var match = matches[0].Value[1..];

        // Replace the comma with a dot, so we can parse the string to a double
        var result = double.TryParse(match.Replace(',', '.'), out var balance);

        if (!result)
            return 0;

        return balance;
    }

    /// <summary>
    /// Gets the email from the page.
    /// </summary>
    /// <param name="page">Page to scrape</param>
    /// <returns>Scraped email address</returns>
    private static string ParseEmail(string page)
    {
        var matches = EmailRegex().Matches(page);

        if (matches.Count == 0)
            return string.Empty;

        return matches[0].Value;
    }

    /// <summary>
    /// Gets the name from the page.
    /// </summary>
    /// <param name="page">Page to scrape</param>
    /// <returns>Scraped name</returns>
    private static string ParseName(string page)
    {
        var matches = NameRegex().Matches(page);

        if (matches.Count == 0)
            return string.Empty;

        return matches[0].Value;
    }

    /// <summary>
    /// Gets the purse name from the page.
    /// </summary>
    /// <param name="page">Page to scrape</param>
    /// <returns>Scraped purse</returns>
    private static string ParsePurse(string page)
    {
        var matches = PurseRegex().Matches(page);

        if (matches.Count == 0)
            return string.Empty;

        return matches[0].Value;
    }

    /// <summary>
    /// Gets the username from the page.
    /// </summary>
    /// <param name="page">Page to scrape</param>
    /// <returns>Scraped username</returns>
    private static string ParseUserName(string page)
    {
        var matches = UserNameRegex().Matches(page);

        if (matches.Count == 0)
            return string.Empty;

        return matches[0].Value;
    }

    /// <summary>
    /// Convert the date string to a <see cref="DateTime"/> object.
    /// </summary>
    /// <param name="date">Date string to convert</param>
    /// <returns><see cref="DateTime"/> of the date string</returns>
    private static DateTime ConvertTransactionDateToDateTime(string date)
    {
        var trimmedDate = date.Trim();

        return DateTime.ParseExact(trimmedDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Convert the price string to a <see cref="double"/> object.
    /// </summary>
    /// <param name="price">Price string to convert</param>
    /// <returns>Converted price</returns>
    private static double ConvertTransactionPriceToDouble(string price)
    {
        var trimmedPrice = price.Trim();

        // Remove the currency symbol from the price
        return double.Parse(trimmedPrice[1..].Replace(',', '.'), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Scrapes all the transaction dates from the page.
    /// </summary>
    /// <param name="page">Page where all the date times will be scraped from</param>
    /// <returns>List of scraped dates</returns>
    private static List<string> ParseTransactionDateTimes(string[] page)
    {
        var indexes = new List<int>();

        // Get all the indexes of the transaction date times
        indexes = page
                    .Select((value, i) => new { i, value })
                    .Where(x => x.value.Contains("trxdatetime"))
                    .Select(x => x.i)
                    .ToList();

        var dateTimes = new List<string>();

        foreach (var i in indexes)
        {
            var dateTime = page[i + 1];
            dateTimes.Add(dateTime);
        }

        return dateTimes;
    }

    /// <summary>
    /// Scrapes all the transaction descriptions from the page.
    /// </summary>
    /// <param name="page">Page where all the date times will be scraped from</param>
    /// <returns>List of scraped descriptions</returns>
    private static List<string> ParseTransactionDescriptions(string[] page)
    {
        var indexes = new List<int>();

        // Get all the indexes of the transaction descriptions
        indexes = page
                    .Select((value, i) => new { i, value })
                    .Where(x => x.value.Contains("trxdescription"))
                    .Select(x => x.i)
                    .ToList();

        var descriptions = new List<string>();

        foreach (var i in indexes)
        {
            var description = page[i + 1];
            descriptions.Add(description);
        }

        return descriptions;
    }

    /// <summary>
    /// Scrapes all the transaction prices from the page.
    /// </summary>
    /// <param name="page">Page where all the date times will be scraped from</param>
    /// <returns>List of scraped prices</returns>
    private static List<string> ParseTransactionPrices(string[] page)
    {
        var indexes = new List<int>();

        // Get all the indexes of the transaction prices
        indexes = page
                    .Select((value, i) => new { i, value })
                    .Where(x => x.value.Contains("trxamount "))
                    .Select(x => x.i)
                    .ToList();

        var prices = new List<string>();

        foreach (var i in indexes)
        {
            var price = page[i + 1];
            prices.Add(price);
        }

        return prices;
    }

    /// <summary>
    /// Scrapes all the transaction locations from the page.
    /// </summary>
    /// <param name="page">Page where all the date times will be scraped from</param>
    /// <returns>List of scraped locations</returns>
    private static List<string> ParseTransactionLocations(string[] page)
    {
        var indexes = new List<int>();

        // Get all the indexes of the transaction locations
        indexes = page
                    .Select((value, i) => new { i, value })
                    .Where(x => x.value.Contains("trxlocation"))
                    .Select(x => x.i)
                    .ToList();

        var locations = new List<string>();

        foreach (var i in indexes)
        {
            var location = page[i + 1];
            locations.Add(location);
        }

        return locations;
    }
}
