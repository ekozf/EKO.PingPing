using EKO.PingPing.Mobile.ViewModels;

namespace EKO.PingPing.Mobile.Views;

public partial class PursePage : ContentPage
{
    public PursePage(PurseViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}