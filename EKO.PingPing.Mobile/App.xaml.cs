using EKO.PingPing.Mobile.Components;
using EKO.PingPing.Mobile.Views;
using EKO.PingPing.Shared;
using MaterialColorUtilities.Maui;
using Microsoft.Maui.Platform;

namespace EKO.PingPing.Mobile;

public partial class App : Application
{
    private readonly MaterialColorService _materialColorService;
    private readonly LoginPage _page;

    public App(MaterialColorService materialColorService, LoginPage page)
    {
        InitializeComponent();

        _materialColorService = materialColorService;

#if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
                //handler.PlatformView.TextCursorDrawable.SetTint(materialColorService.SchemeMaui.Primary.ToPlatform());
                handler.PlatformView.TextCursorDrawable.SetTint(Colors.Transparent.ToPlatform());
            }
        });
#endif
        _page = page;

        MainPage = new AppShell();
    }

    protected async override void OnStart()
    {
        if (string.IsNullOrEmpty(await SecureStorage.GetAsync(AppConsts.COOKIE_KEY)))
        {
            MainPage = _page;
            return;
        }

        if (await SecureStorage.GetAsync(AppConsts.COOKIE_KEY) != null)
        {
            await Shell.Current.GoToAsync($"//{nameof(PursePage)}");
        }

#if ANDROID
        Platform.CurrentActivity.Window.SetNavigationBarColor(_materialColorService.SchemeMaui.Surface2.ToPlatform());
#endif

        base.OnStart();
    }
}
