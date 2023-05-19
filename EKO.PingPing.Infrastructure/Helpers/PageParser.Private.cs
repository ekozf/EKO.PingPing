using System.Globalization;

namespace EKO.PingPing.Infrastructure.Helpers;

internal static partial class PageParser
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
        var result = double.TryParse(match.Replace(',', '.'), NumberFormatInfo.InvariantInfo, out var balance);

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
    private static DateTime ConvertStringDateToDateTime(string date, bool usesDashes = true)
    {
        var trimmedDate = date.Trim();

        if (usesDashes)
            return DateTime.ParseExact(trimmedDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
        else
            return DateTime.ParseExact(trimmedDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
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
        return double.Parse(trimmedPrice[1..].Replace(',', '.'), NumberFormatInfo.InvariantInfo);
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

    /// <summary>
    /// Parses the session date times from the page.
    /// </summary>
    /// <param name="page">Page where all the date times will be scraped from</param>
    /// <returns>List of scraped <see cref="DateTime"/></returns>
    private static List<string> ParseSessionDateTimes(string[] page)
    {
        var indexes = new List<int>();

        // Get all the indexes of the session date times
        indexes = page
                    .Select((value, i) => new { i, value })
                    .Where(x => x.value.Contains("nowrap"))
                    .Select(x => x.i)
                    .ToList();

        var dateTimes = new List<string>();

        // Skip every other index because it's the IP address, which we don't need
        var skipNext = false;

        // Skip the first index because it's not relevant
        foreach (var i in indexes.Skip(1))
        {
            if (skipNext)
            {
                skipNext = false;
                continue;
            }

            var tag = page[i];

            var tdTagEnd = tag.IndexOf('>', StringComparison.Ordinal) + 1;
            var nextTagBegin = tag.IndexOf('<', tdTagEnd);

            // Remove the HTML tags
            var dateTime = tag[tdTagEnd..nextTagBegin];

            dateTimes.Add(dateTime);

            skipNext = true;
        }

        return dateTimes;
    }

    /// <summary>
    /// Parses the session User Agents from the page.
    /// </summary>
    /// <param name="page">Page where all the date times will be scraped from</param>
    /// <returns>List of scraped user agents</returns>
    private static List<string> ParseSessionUserAgents(string[] page)
    {
        var indexes = new List<int>();

        // Get all the indexes of the session user agents
        indexes = page
                    .Select((value, i) => new { i, value })
                    .Where(x => x.value.Contains("nowrap"))
                    .Select(x => x.i)
                    .ToList();

        var userAgents = new List<string>();

        // Skip every other index because it's the IP address, which we don't need.
        // This time, we need to skip the first index because it's the date time.
        // The user agent field is after the IP address field.
        var skipNext = true;

        // Skip the first index because it's not relevant
        foreach (var i in indexes.Skip(1))
        {
            if (skipNext)
            {
                skipNext = false;
                continue;
            }

            var tag = page[i + 1];

            var tdTagEnd = tag.IndexOf('>', StringComparison.Ordinal) + 1;
            var nextTagBegin = tag.IndexOf('<', tdTagEnd);

            // Remove the HTML tags
            var userAgent = tag[tdTagEnd..nextTagBegin];

            userAgents.Add(userAgent);

            skipNext = true;
        }

        return userAgents;
    }

    /// <summary>
    /// Parses the session Ids from the page.
    /// </summary>
    /// <param name="responsePage">Page where all the date times will be scraped from</param>
    /// <returns>List of scraped user agents</returns>
    private static List<string> ParseSessionIds(string[] responsePage)
    {
        var indexes = new List<int>();

        // Get all the indexes of the session ids
        indexes = responsePage
                    .Select((value, i) => new { i, value })
                    .Where(x => x.value.Contains("delete"))
                    .Select(x => x.i)
                    .ToList();

        var sessionIds = new List<string>();

        foreach (var i in indexes)
        {
            var tag = responsePage[i];

            var valueTagBegin = tag.IndexOf("value=\"", StringComparison.Ordinal) + 7;
            var valueTagEnd = tag.IndexOf("\"", valueTagBegin, StringComparison.Ordinal);

            var sessionId = tag[valueTagBegin..valueTagEnd];

            sessionIds.Add(sessionId);
        }

        return sessionIds;
    }
}
