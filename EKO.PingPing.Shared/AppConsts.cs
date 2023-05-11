namespace EKO.PingPing.Shared;

/// <summary>
/// Class that contains all the constants used in the application.
/// </summary>
public static class AppConsts
{
    /// <summary>
    /// Key that will be used to store the cookie in the secure storage.
    /// </summary>
    public const string COOKIE_KEY = "PINGPING_AUTH_COOKIE";

    /// <summary>
    /// Base URL of the PingPing website.
    /// </summary>
    public const string BASE_URL = "https://uhasselt-pxl.mynetpay.be/";

    /// <summary>
    /// User agent that will be used in the requests. Added to prevent the website from returning an error. End of the user agent is the version of the application.
    /// </summary>
    public const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 PingDingApp/1.0.0";
}
