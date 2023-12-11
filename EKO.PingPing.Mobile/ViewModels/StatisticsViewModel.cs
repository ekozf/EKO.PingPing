using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Shared.Models;
using static EKO.PingPing.Shared.AppConsts;

namespace EKO.PingPing.Mobile.ViewModels;

public sealed partial class StatisticsViewModel : ObservableObject
{
    private readonly IPingPingService _pingPingService;

    [ObservableProperty]
    private string _totalSpentString = string.Empty;

    [ObservableProperty]
    private string _totalMonthlySpentString = string.Empty;

    [ObservableProperty]
    private string _totalPreviouslyMonthlySpentString = string.Empty;

    [ObservableProperty]
    private string _totalYearlySpentString = string.Empty;

    public StatisticsViewModel(IPingPingService pingPingService)
    {
        _pingPingService = pingPingService;
    }

    [RelayCommand]
    private async Task LoadStatistics()
    {
        var totalSpent = await GetTotalSpent();
        TotalSpentString = $"€ {totalSpent:F2}";

        var monthlySpent = await GetMonthlySpent(DateTime.Now.Month);
        TotalMonthlySpentString = $"€ {monthlySpent:F2}";

        var previousMonth = DateTime.Now.Month - 1;

        if (previousMonth is 0)
            previousMonth = 12;

        var previousMonthSpent = await GetMonthlySpent(previousMonth);
        TotalPreviouslyMonthlySpentString = $"€ {previousMonthSpent:F2}";

        var yearlySpent = await GetYearlySpent();
        TotalYearlySpentString = $"€ {yearlySpent:F2}";
    }

    /// <summary>
    /// Load every transaction that has been made.
    /// </summary>
    /// <returns>Total amount of money spent.</returns>
    private async Task<double> GetTotalSpent()
    {
        var allTransactions = await GetTransactions();

        return SumAndRound(allTransactions);
    }

    /// <summary>
    /// Load all the transactions for a given month.
    /// </summary>
    /// <param name="month">Month to load the transactions from</param>
    /// <returns>Balance spent this month</returns>
    private async Task<double> GetMonthlySpent(int month)
    {
        var transactions = await GetTransactions();

        var monthlyTransactions = transactions
            .Where(x => x.Date.Month == month && x.Date.Year == DateTime.Now.Year);

        return SumAndRound(monthlyTransactions);
    }

    /// <summary>
    /// Load all the transactions from the beginning of the school year.
    /// </summary>
    /// <returns>Balance spent this school year</returns>
    private async Task<double> GetYearlySpent()
    {
        var transactions = await GetTransactions();

        var yearlyTransactions = transactions
            .Where(x => new DateTime(DateTime.Now.Year, 9, 1) < x.Date)
            .OrderByDescending(x => x.Date);

        return SumAndRound(yearlyTransactions);
    }

    /// <summary>
    /// Get all the transactions.
    /// </summary>
    /// <returns>All loaded transactions</returns>
    private async Task<IReadOnlyList<TransactionModel>> GetTransactions()
    {
        var hasReachedEnd = false;
        var currentPage = 0;
        var transactionsList = new List<TransactionModel>();

        while (!hasReachedEnd)
        {
            // Get all transactions as tasks and wait for them all to concurrently finish
            var results = await Task.WhenAll(GetTransactionsAll(currentPage, AMOUNT_TO_LOAD_PAGES));

            // Add all the transactions to the list
            transactionsList.AddRange(results.SelectMany(x => x.Transactions));

            // If any of the results has less than 25 transactions, we have reached the end
            if (results.Any(x => x.Transactions.Count < 25)) // 25 is the max amount of transactions per page
                hasReachedEnd = true;

            // Increase the current page by the amount of pages we loaded
            currentPage += AMOUNT_TO_LOAD_PAGES;
        }

        return transactionsList;
    }

    /// <summary>
    /// Get all the transactions for a given page.
    /// </summary>
    /// <param name="page">Transaction history page number</param>
    /// <returns>List of tasks of transactions</returns>
    private IReadOnlyList<Task<PagedTransactionModel>> GetTransactionsAll(int page, int amountOfPagesToLoad)
    {
        var currentPage = page;

        var transactionsTasks = new List<Task<PagedTransactionModel>>();

        while (currentPage != page + amountOfPagesToLoad)
        {
            transactionsTasks.Add(_pingPingService.GetTransactions(currentPage));

            currentPage++;
        }

        return transactionsTasks.AsReadOnly();
    }

    /// <summary>
    /// Sum all the negative prices and round the result to 2 decimals.
    /// </summary>
    /// <param name="transactions">List of transactions</param>
    /// <returns>Summed up and rounded total value of transactions</returns>
    private static double SumAndRound(IEnumerable<TransactionModel> transactions)
    {
        return Math.Round(transactions.Where(x => x.Price < 0).Sum(x => Math.Abs(x.Price)), 2);
    }
}
