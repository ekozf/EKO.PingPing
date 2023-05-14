using EKO.PingPing.Mobile.ViewModels;

namespace EKO.PingPing.Mobile.Views;

public partial class SessionsPage : ContentPage
{
    public SessionsPage(SessionsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}