using EKO.PingPing.Shared;

namespace EKO.PingPing.Infrastructure.Extensions;

/// <summary>
/// Extra methods for <see cref="HttpResponseMessage"/>.
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Similar to <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/> but logs out the user if the response is not successful.
    /// </summary>
    /// <param name="msg"><see cref="HttpResponseMessage"/></param>
    /// <returns><see cref="HttpResponseMessage"/> if the call was successful</returns>
    public static async Task<HttpResponseMessage> LogOutUserIfInvalidResponse(this HttpResponseMessage msg)
    {
        // Check if the response is successful.
        if (msg.IsSuccessStatusCode)
            return msg;

        // If the response is not successful, we will log out the user.
        SecureStorage.Default.Remove(AppConsts.COOKIE_KEY);

        // We will also redirect the user to the login page.
        await Shell.Current.GoToAsync("//ProfilePage?ForceLogoutUser=true");

        return msg;
    }
}
