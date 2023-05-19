using EKO.PingPing.Mobile.Components;
using EKO.PingPing.Mobile.Views;
using EKO.PingPing.Shared;
using MaterialColorUtilities.Maui;
using Microsoft.Maui.Platform;

namespace EKO.PingPing.Mobile;

public partial class App : Application
{
    private readonly MaterialColorService _materialColorService;

    public App(MaterialColorService materialColorService)
    {
        InitializeComponent();

#if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
                handler.PlatformView.TextCursorDrawable.SetTint(materialColorService.SchemeMaui.Primary.ToPlatform());
            }
        });
#endif
        MainPage = new AppShell();

        _materialColorService = materialColorService;
    }

    protected async override void OnStart()
    {
        if (await SecureStorage.GetAsync(AppConsts.COOKIE_KEY) != null)
        {
#if WINDOWS
            await Task.Delay(250);
#endif
            await Shell.Current.GoToAsync($"//{nameof(PursePage)}");
        }

#if ANDROID
        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Window.SetNavigationBarColor(_materialColorService.SchemeMaui.Surface2.ToPlatform());
#endif

        base.OnStart();
    }
}
