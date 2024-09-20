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

        var monthlySpent = await GetMonthlySpent(DateTime.Today.Month, DateTime.Today.Year);
        TotalMonthlySpentString = $"€ {monthlySpent:F2}";

        var previousMonth = DateTime.Today.Month - 1;
        var year = DateTime.Today.Year;

        if (DateTime.Today.Month == 1)
        {
            previousMonth = 12;
            year = DateTime.Today.Year - 1;
        }

        var previousMonthSpent = await GetMonthlySpent(previousMonth, year);
        TotalPreviouslyMonthlySpentString = $"€ {previousMonthSpent:F2}";
    

        var yearlySpent = await GetYearlySpent(year);
        TotalYearlySpentString = $"€ {yearlySpent:F2}";
    }

    /// <summary>
    /// Load every transaction that has been made.
    /// </summary>
    /// <returns>Total amount of money spent.</returns>
    private async Task<double> GetTotalSpent()
    {
        var allTransactions = await _pingPingService.GetTransactionsByDate(new DateTime(2004, 8, 7));

        return SumAndRound(allTransactions.Transactions);
    }

    /// <summary>
    /// Load all the transactions for a given month.
    /// </summary>
    /// <param name="month">Month to load the transactions from</param>
    /// <returns>Balance spent this month</returns>
    private async Task<double> GetMonthlySpent(int month, int year)
    {
        // Get the start of the current month
        var firstDayCurrentMonth = new DateTime(year, month, 1);

        var transactions = await _pingPingService.GetTransactionsByDate(firstDayCurrentMonth);

        if (month < DateTime.Today.Month)
        {
            var transactionsPreviousMonth = transactions.Transactions.Where(x => x.Date < firstDayCurrentMonth).ToList();

            return SumAndRound(transactionsPreviousMonth);
        }

        return SumAndRound(transactions.Transactions);
    }

    /// <summary>
    /// Load all the transactions from the beginning of the school year.
    /// </summary>
    /// <returns>Balance spent this school year</returns>
    private async Task<double> GetYearlySpent(int year)
    {
        var datedTransactions = await _pingPingService.GetTransactionsByDate(new DateTime(year, 9, 1));

        bool isBeforeSeptember = DateTime.Now.Date < new DateTime(year, 9, 1);

        List<TransactionModel> transactions;

        if (isBeforeSeptember)
        {
            transactions = datedTransactions.Transactions.Where(x => x.Date > new DateTime(year - 1, 9, 1)).ToList();
        }
        else
        {
            transactions = datedTransactions.Transactions.Where(x => x.Date > new DateTime(year, 9, 1)).ToList();
        }

        return SumAndRound(transactions);
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
