using EKO.PingPing.Mobile.ViewModels;

namespace EKO.PingPing.Mobile.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}