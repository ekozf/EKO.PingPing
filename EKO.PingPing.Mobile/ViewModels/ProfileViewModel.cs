using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Mobile.Views;
using EKO.PingPing.Shared.Models;

namespace EKO.PingPing.Mobile.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly IPingPingService _pingPingService;

    [ObservableProperty]
    private PurseModel _purse;

    public ProfileViewModel(IPingPingService pingPingService)
    {
        _pingPingService = pingPingService;
    }

    [RelayCommand]
    private async Task LoadUserData()
    {
        var purse = await _pingPingService.GetUserPurse();

        if (purse is null)
            return;

        Purse = purse;
    }

    [RelayCommand]
    private async Task LogOutUser()
    {
        var wasLoggedOut = await _pingPingService.DoUserLogout();

        if (wasLoggedOut)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Something went wrong while logging out.", "OK");
        }
    }

    [RelayCommand]
    private async Task ViewSessions()
    {
        await Shell.Current.GoToAsync(nameof(SessionsPage));
    }
}
