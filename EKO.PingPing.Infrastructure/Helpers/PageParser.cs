using EKO.PingPing.Shared.Models;
using EKO.PingPing.Shared.Responses;

namespace EKO.PingPing.Infrastructure.Helpers;

/// <summary>
/// Web scraper that parses the HTML page and returns the desired data.
/// </summary>
internal static partial class PageParser
{
    /// <summary>
    /// Checks if the login was valid.
    /// </summary>
    /// <param name="page">Page to scrape</param>
    /// <returns>true if the login has redirected to the main page, false if we got an error.</returns>
    internal static bool LoginWasValid(string page)
    {
        return !page.Contains("Error Invalid Password", StringComparison.InvariantCultureIgnoreCase)
            && !page.Contains("Error Specified account does not exist", StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Scrapes the page and returns the data of the user's purse.
    /// </summary>
    /// <param name="response"><see cref="PageResponse"/> with the retrieved page</param>
    /// <returns>Scraped and parsed purse.</returns>
    internal static PurseModel ParseUserPurse(PageResponse response)
    {
        var page = response.Page;

        return new PurseModel
        {
            UserName = ParseUserName(page),
            Name = ParseName(page),
            Email = ParseEmail(page),
            Purse = ParsePurse(page),
            Balance = ParseBalance(page)
        };
    }

    /// <summary>
    /// Scrapes the page and returns the data of the user's transactions.
    /// </summary>
    /// <param name="response"><see cref="PageResponse"/> with the retrieved page</param>
    /// <param name="page">Page number to get</param>
    /// <returns>List of transactions that the user has made</returns>
    internal static PagedTransactionModel ParseTransactions(PageResponse response, int page)
    {
        // Remove the tabs from the page and split it into lines
        var responsePage = response.Page.Replace("\t", "").Split('\n');

        var dateTimes = ParseTransactionDateTimes(responsePage);
        var descriptions = ParseTransactionDescriptions(responsePage);
        var prices = ParseTransactionPrices(responsePage);
        var locations = ParseTransactionLocations(responsePage);

        // Combine the data into one list so we can enumerate them together
        var dataZip = dateTimes
                        .Zip(descriptions, (Date, Description) => new { Date, Description })
                        .Zip(prices, (x, Price) => new { x.Date, x.Description, Price })
                        .Zip(locations, (x, Location) => new { x.Date, x.Description, x.Price, Location });

        var transactions = new PagedTransactionModel(page);

        foreach (var entry in dataZip)
        {
            transactions.Transactions.Add(new TransactionModel
            {
                Date = ConvertStringDateToDateTime(entry.Date),
                Description = entry.Description,
                Price = ConvertTransactionPriceToDouble(entry.Price),
                Location = entry.Location
            });
        }

        // API returns 25 transactions per page, so if we have less than that, we have reached the end.
        if (transactions.Transactions.Count < 25)
        {
            transactions.HasReachedEnd = true;
        }

        return transactions;
    }

    /// <summary>
    /// Parses the page and returns the data of the user's sessions.
    /// </summary>
    /// <param name="sessions"><see cref="PageResponse"/> with the retrieved page</param>
    /// <returns><see cref="SessionsModelList"/> that has all the current user sessions.</returns>
    internal static SessionsModelList ParseUserSessions(PageResponse sessions)
    {
        var responsePage = sessions.Page.Replace("\t", "").Split('\n');

        // DateTimes list has an empty entry somewhere, get its index
        var dateTimes = ParseSessionDateTimes(responsePage);

        var emptyIndex = dateTimes.FindIndex(x => x?.Length == 0);

        // Clear the empty entry
        var userAgents = ParseSessionUserAgents(responsePage);

        // Remove the empty entry
        userAgents[emptyIndex] = string.Empty;

        // Add a empty entry to the list so we can enumerate them together
        var sessionIds = ParseSessionIds(responsePage);

        sessionIds.Insert(emptyIndex, string.Empty);

        // Combine the data into one list so we can enumerate them together
        var dataZip = dateTimes
                        .Zip(userAgents, (Date, UserAgent) => new { Date, UserAgent })
                        .Zip(sessionIds, (x, SessionId) => new { x.Date, x.UserAgent, SessionId });

        var sessionModelList = new SessionsModelList();

        foreach (var entry in dataZip)
        {
            if (string.IsNullOrWhiteSpace(entry.Date))
                continue;

            sessionModelList.Sessions.Add(new SessionsModel
            {
                LastActiveDate = ConvertStringDateToDateTime(entry.Date, usesDashes: false),
                UserAgent = entry.UserAgent,
                SessionId = entry.SessionId
            });
        }

        return sessionModelList;
    }
}
