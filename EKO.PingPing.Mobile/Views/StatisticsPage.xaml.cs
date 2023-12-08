using EKO.PingPing.Mobile.ViewModels;

namespace EKO.PingPing.Mobile.Views;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage(StatisticsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}