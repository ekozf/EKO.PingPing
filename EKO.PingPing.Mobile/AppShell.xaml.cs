using EKO.PingPing.Mobile.Views;

namespace EKO.PingPing.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register detail pages, which allows to navigate to them.
        Routing.RegisterRoute(nameof(SessionsPage), typeof(SessionsPage));
    }
}
