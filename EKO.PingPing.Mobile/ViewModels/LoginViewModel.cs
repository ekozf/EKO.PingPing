using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Mobile.Resources.Localization;

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
            RedirectToPurse();
        }
    }

    [RelayCommand]
    private async Task DoLogin()
    {
        await _pingPingService.DoUserLogin(UserName, Password);

        var isLoggedIn = await _pingPingService.IsUserLoggedIn();

        if (isLoggedIn)
        {
            RedirectToPurse();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(
                AppResources.ValidationError_UserLoginFailed_Title,
                AppResources.ValidationError_UserLoginFailed_Message,
                "OK");
        }
    }

    private void RedirectToPurse()
    {
        Application.Current.MainPage = new AppShell();
    }
}
