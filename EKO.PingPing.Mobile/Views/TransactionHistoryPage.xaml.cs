using EKO.PingPing.Mobile.ViewModels;

namespace EKO.PingPing.Mobile.Views;

public partial class TransactionHistoryPage : ContentPage
{
    public TransactionHistoryPage(TransactionHistoryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}