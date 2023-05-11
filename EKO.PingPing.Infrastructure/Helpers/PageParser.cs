﻿using EKO.PingPing.Shared.Models;
using EKO.PingPing.Shared.Responses;

namespace EKO.PingPing.Infrastructure.Helpers;

/// <summary>
/// Web scraper that parses the HTML page and returns the desired data.
/// </summary>
public static partial class PageParser
{
    /// <summary>
    /// Checks if the login was valid.
    /// </summary>
    /// <param name="page">Page to scrape</param>
    /// <returns>true if the login has redirected to the main page, false if we got an error.</returns>
    public static bool LoginWasValid(string page)
    {
        return !page.Contains("Error Invalid Password");
    }

    /// <summary>
    /// Scrapes the page and returns the data of the user's purse.
    /// </summary>
    /// <param name="response"><see cref="PageResponse"/> with the retrieved page</param>
    /// <returns>Scraped and parsed purse.</returns>
    public static PurseModel ParseUserPurse(PageResponse response)
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
    public static PagedTransactionModel ParseTransactions(PageResponse response, int page)
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
                Date = ConvertTransactionDateToDateTime(entry.Date),
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
}