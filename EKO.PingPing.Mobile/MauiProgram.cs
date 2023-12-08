using CommunityToolkit.Maui;
using EKO.PingPing.Infrastructure.Caching;
using EKO.PingPing.Infrastructure.Caching.Contracts;
using EKO.PingPing.Infrastructure.Services;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Mobile.ViewModels;
using EKO.PingPing.Mobile.Views;
using EKO.PingPing.Shared.Models;
using MaterialColorUtilities.Maui;
using Microsoft.Extensions.Logging;

namespace EKO.PingPing.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMaterialColors()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Robot-Regular.ttf", "RobotoRegular");
                fonts.AddFont("Roboto-Italic.ttf", "RobotoItalic");
                fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                fonts.AddFont("Roboto-MediumItalic.ttf", "RobotoMediumItalic");
                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                fonts.AddFont("Roboto-BoldItalic.ttf", "RobotoBoldItalic");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // DI for the Infrastructure project
        builder.Services.AddSingleton<IRequestService, PingPingRequestService>();
        builder.Services.AddSingleton<IPingPingService, PingPingService>();
        builder.Services.AddSingleton<ICachedRepository<ExpiringModelBase>, CachedModelRepository>();

        // DI for the Mobile project
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LoginViewModel>();

        builder.Services.AddTransient<PursePage>();
        builder.Services.AddTransient<PurseViewModel>();

        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<ProfileViewModel>();

        builder.Services.AddTransient<TransactionHistoryPage>();
        builder.Services.AddTransient<TransactionHistoryViewModel>();

        builder.Services.AddTransient<SessionsPage>();
        builder.Services.AddTransient<SessionsViewModel>();

        builder.Services.AddTransient<StatisticsPage>();
        builder.Services.AddTransient<StatisticsViewModel>();

        return builder.Build();
    }
}
