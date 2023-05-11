using EKO.PingPing.Mobile.ViewModels;

namespace EKO.PingPing.Mobile.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}