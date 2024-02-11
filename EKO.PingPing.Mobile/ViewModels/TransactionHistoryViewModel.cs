using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Shared.Models;
using System.Collections.ObjectModel;

namespace EKO.PingPing.Mobile.ViewModels;

public partial class TransactionHistoryViewModel : ObservableObject
{
    private readonly IPingPingService _pingPingService;

    [ObservableProperty]
    private ObservableCollection<TransactionModel> _transactions;

    private int _currentPage = 0;
    private bool _hasReachedEnd = false;

    public TransactionHistoryViewModel(IPingPingService pingPingService)
    {
        _pingPingService = pingPingService;
        _transactions = new();
    }

    [RelayCommand]
    private async Task LoadTransactions()
    {
        if (Transactions.Count > 0)
            return;

        var transactions = await _pingPingService.GetTransactions(0);

        if (transactions is null)
            return;

        foreach (var transaction in transactions.Transactions)
        {
            Transactions.Add(transaction);
        }
    }

    [RelayCommand]
    private async Task RefreshTransactions()
    {
        var transactions = await _pingPingService.GetTransactions(0, forced: true);

        if (transactions is null)
            return;

        Transactions.Clear();

        foreach (var transaction in transactions.Transactions)
        {
            Transactions.Add(transaction);
        }
    }

    [RelayCommand]
    private async Task LoadNextTransactionPage()
    {
        if (_hasReachedEnd)
            return;

        var transactions = await _pingPingService.GetTransactions(_currentPage);

        if (transactions is null)
            return;

        foreach (var transaction in transactions.Transactions)
        {
            Transactions.Add(transaction);
        }
        _currentPage += 1;
    }
}
