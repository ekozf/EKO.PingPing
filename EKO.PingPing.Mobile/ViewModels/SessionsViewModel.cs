using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Shared.Models;

namespace EKO.PingPing.Mobile.ViewModels;

public partial class SessionsViewModel : ObservableObject
{
    private readonly IPingPingService _pingPingService;

    [ObservableProperty]
    private List<SessionsModel> _sessions;

    public SessionsViewModel(IPingPingService pingPingService)
    {
        _pingPingService = pingPingService;
    }

    [RelayCommand]
    private async Task LoadUserSessions()
    {
        var result = await _pingPingService.GetUserSessions();

        if (result is null)
            return;

        Sessions = result.Sessions;
    }
}
