using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Shared.Models;

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

        var monthlySpent = await GetMonthlySpent(DateTime.Now.Month, DateTime.Now.Month);
        TotalMonthlySpentString = $"€ {monthlySpent:F2}";

        var previousMonth = DateTime.Now.Month - 1;
        var year = DateTime.Now.Year;

        if (DateTime.Now.Month == 1)
        {
            year -= 1;
        }

        if (previousMonth is 0)
            previousMonth = 12;

        var previousMonthSpent = await GetMonthlySpent(previousMonth, year);
        TotalPreviouslyMonthlySpentString = $"€ {previousMonthSpent:F2}";
    
        var yearlySpent = await GetYearlySpent();
        TotalYearlySpentString = $"€ {yearlySpent:F2}";
    }

    private async Task<double> GetTotalSpent()
    {
        var allTransactions = await GetTransactions();

        return SumAndRound(allTransactions);
    }

    private async Task<double> GetMonthlySpent(int month, int year)
    {
        var transactions = await GetTransactions();

        bool Januari = DateTime.Now.Date < new DateTime(DateTime.Now.Year, 1, 1);
        var monthlyTransactions = transactions
            .Where(x => x.Date.Month == month && x.Date.Year == year)
            .ToList();

        return SumAndRound(monthlyTransactions);
    }

    private async Task<double> GetYearlySpent()
    {
        var transactions = await GetTransactions();

        bool isBeforeSeptember = DateTime.Now.Date < new DateTime(DateTime.Now.Year, 9, 1);
        var yearlyTransactions = transactions
            .OrderByDescending(x => x.Date)
            .Where(x => isBeforeSeptember ? x.Date < new DateTime(DateTime.Now.Year - 1, 9, 1) : x.Date < new DateTime(DateTime.Now.Year, 9, 1))
            .ToList();

        return SumAndRound(yearlyTransactions);
    }

    private async Task<IReadOnlyList<TransactionModel>> GetTransactions()
    {
        var hasReachedEnd = false;
        var currentPage = 0;
        var transactionsList = new List<TransactionModel>();

        while (!hasReachedEnd)
        {
            var transactions = await _pingPingService.GetTransactions(currentPage);

            if (transactions is null || transactions.Transactions.Count is 0)
                break;

            foreach (var transaction in transactions.Transactions)
            {
                transactionsList.Add(transaction);
            }

            currentPage++;
        }

        return transactionsList;
    }

    private static double SumAndRound(IReadOnlyList<TransactionModel> transactions)
    {
        return Math.Round(transactions.Where(x => x.Price < 0).Sum(x => Math.Abs(x.Price)), 2);
    }
}
