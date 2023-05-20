using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Mobile.Views;
using EKO.PingPing.Shared.Models;

namespace EKO.PingPing.Mobile.ViewModels;

[QueryProperty(nameof(ForceLogoutUser), nameof(ForceLogoutUser))]
public partial class ProfileViewModel : ObservableObject
{
    private readonly IPingPingService _pingPingService;
    private readonly LoginPage _loginPage;

    [ObservableProperty]
    private PurseModel _purse;

    [ObservableProperty]
    private bool _forceLogoutUser;

    public ProfileViewModel(IPingPingService pingPingService, LoginPage loginPage)
    {
        _pingPingService = pingPingService;
        _loginPage = loginPage;
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
            RedirectLoggedOutUser();
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

    private void RedirectLoggedOutUser()
    {
        Application.Current.MainPage = _loginPage;
    }

    partial void OnForceLogoutUserChanged(bool value)
    {
        if (value)
        {
            RedirectLoggedOutUser();
        }
    }
}
