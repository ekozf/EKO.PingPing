using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Shared.Models;
using System.Transactions;

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

    private async Task<double> GetTotalSpent()
    {
        var hasReachedEnd = false;
        var currentPage = 0;
        var allTransactions = new List<TransactionModel>();

        while (!hasReachedEnd)
        {
            var transactions = await _pingPingService.GetTransactions(currentPage);

            if (transactions is null || transactions.Transactions.Count is 0)
                break;

            foreach (var transaction in transactions.Transactions)
            {
                allTransactions.Add(transaction);
            }

            currentPage++;
            hasReachedEnd = transactions.HasReachedEnd;
        }

        return Math.Round(allTransactions.Where(x => x.Price < 0).Sum(x => Math.Abs(x.Price)), 2);
    }

    private async Task<double> GetMonthlySpent(int month)
    {
        var hasReachedEnd = false;
        var currentPage = 0;
        var monthlyTransactions = new List<TransactionModel>();

        while (!hasReachedEnd)
        {
            var transactions = await _pingPingService.GetTransactions(currentPage);

            if (transactions is null || transactions.Transactions.Count is 0)
                break;

            foreach (var transaction in transactions.Transactions)
            {
                monthlyTransactions.Add(transaction);
            }

            currentPage++;
            hasReachedEnd = transactions.HasReachedEnd;
        }

        monthlyTransactions = monthlyTransactions.Where(x => x.Date.Month == month && x.Date.Year == DateTime.Now.Year).ToList();

        return Math.Round(monthlyTransactions.Where(x => x.Price < 0).Sum(x => Math.Abs(x.Price)), 2);
    }

    private async Task<double> GetYearlySpent()
    {
        var hasReachedEnd = false;
        var currentPage = 0;
        var yearlyTransactions = new List<TransactionModel>();

        while (!hasReachedEnd)
        {
            var transactions = await _pingPingService.GetTransactions(currentPage);

            if (transactions is null || transactions.Transactions.Count is 0)
                break;

            foreach (var transaction in transactions.Transactions)
            {
                yearlyTransactions.Add(transaction);
            }

            currentPage++;
            hasReachedEnd = transactions.HasReachedEnd;
        }

        yearlyTransactions = yearlyTransactions.OrderByDescending(x => x.Date).Where(x => new DateTime(DateTime.Now.Year, 9, 1) < x.Date).ToList();

        return Math.Round(yearlyTransactions.Where(x => x.Price < 0).Sum(x => Math.Abs(x.Price)), 2);
    }
}
