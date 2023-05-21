using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Shared.Models;

namespace EKO.PingPing.Mobile.ViewModels;

public partial class PurseViewModel : ObservableObject
{
    private readonly IPingPingService _pingPingService;

    private PurseModel _purse;

    [ObservableProperty]
    private PagedTransactionModel _transactions;

    [ObservableProperty]
    private double _balance;

    [ObservableProperty]
    private string _userName;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _email;

    public PurseViewModel(IPingPingService pingPingService)
    {
        _pingPingService = pingPingService;
    }

    [RelayCommand]
    private async Task LoadPurse()
    {
        _purse = await _pingPingService.GetUserPurse();
        Transactions = await _pingPingService.GetRecentTransactions();

        if (_purse is null)
            return;

        UserName = _purse.UserName;

        Name = _purse.Name;

        Email = _purse.Email;

        await AnimateBalanceTo(_purse.Balance);
    }

    [RelayCommand]
    private void ResetDisplayBalance()
    {
        Balance = 0;
    }

    [RelayCommand]
    private async Task RefreshPurse()
    {
        _purse = await _pingPingService.GetUserPurse(forced: true);
        Transactions = await _pingPingService.GetRecentTransactions(forced: true);

        if (_purse is null)
            return;

        if (Transactions is null)
            return;

        await AnimateBalanceTo(_purse.Balance);
    }

    private async Task AnimateBalanceTo(double value)
    {
        Balance = 0.0;

        var balance = 0.0;

        for (var i = 0; i < 10; i++)
        {
            balance += value / 10;
            Balance = balance;
            await Task.Delay(40);
        }

        Balance = value;
    }
}
