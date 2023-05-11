using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Mobile.Views;

namespace EKO.PingPing.Mobile.ViewModels;

/// <summary>
/// ViewModel for LoginPage.xaml
/// </summary>
public sealed partial class LoginViewModel : ObservableObject
{
    private readonly IPingPingService _pingPingService;

    [ObservableProperty]
    private string _userName;

    [ObservableProperty]
    private string _password;

    public LoginViewModel(IPingPingService pingPingService)
    {
        _pingPingService = pingPingService;
    }

    [RelayCommand]
    private async Task CheckUserLoggedIn()
    {
        var isLoggedIn = await _pingPingService.IsUserLoggedIn();

        if (isLoggedIn)
        {
            await RedirectToPurse();
        }
    }

    [RelayCommand]
    private async Task DoLogin()
    {
        await _pingPingService.DoUserLogin(UserName, Password);

        var isLoggedIn = await _pingPingService.IsUserLoggedIn();

        if (isLoggedIn)
        {
            await RedirectToPurse();
        }
        else
        {
            await Shell.Current.DisplayAlert("Login Failed", "Please check your credentials and try again.", "OK");
        }
    }

    private async Task RedirectToPurse()
    {
#if WINDOWS
        await Task.Delay(250);
#endif
        await Shell.Current.GoToAsync($"//{nameof(PursePage)}");
    }
}
