using EKO.PingPing.Shared;

namespace EKO.PingPing.Infrastructure.Extensions;

/// <summary>
/// Extra methods for <see cref="HttpResponseMessage"/>.
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Same as <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/> but also logs out the user if the response is not successful.
    /// </summary>
    /// <param name="msg"><see cref="HttpResponseMessage"/></param>
    /// <returns><see cref="HttpResponseMessage"/> if the call was successful</returns>
    public static async Task<HttpResponseMessage> EnsureSuccessStatusCodeOrLogoutUser(this HttpResponseMessage msg)
    {
        try
        {
            // If the response is successful, we will return it.
            return msg.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            // If the response is not successful, we will log out the user.
            SecureStorage.Default.Remove(AppConsts.COOKIE_KEY);

            // We will also redirect the user to the login page.
            await Shell.Current.GoToAsync("//LoginPage");

            return msg;
        }
    }
}
