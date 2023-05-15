using EKO.PingPing.Shared.Models;

namespace EKO.PingPing.Infrastructure.Helpers;

internal static class UserAgentParser
{
    /// <summary>
    /// Parses the user agent string and returns a <see cref="ReadableUserAgent"/> object
    /// </summary>
    /// <param name="userAgent">user agent to parse</param>
    /// <returns><see cref="ReadableUserAgent"/> or null</returns>
    internal static ReadableUserAgent? GetUserAgent(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return null;

        // Get the part between the first '(' and the first ')'
        var part = userAgent[userAgent.IndexOf('(')..userAgent.IndexOf(')')];

        // Try to get the browser
        var browser = GetBrowser(userAgent);

        // Try to get the operating system
        var os = GetOperatingSystem(part, userAgent.Contains("Mobile", StringComparison.InvariantCultureIgnoreCase));

        return new ReadableUserAgent
        {
            Browser = browser,
            Os = os,
        };
    }

    /// <summary>
    /// Helper method to get the browser from the user agent
    /// </summary>
    /// <param name="userAgent">user agent to search</param>
    /// <returns>Guessed browser from the user agent</returns>
    private static string GetBrowser(string userAgent)
    {
        // This is not a good way to do this, but it that does not matter for that much
        // since this is just to show the user something that they can understand
        // when viewing the session list

        if (userAgent.Contains("PingDingApp", StringComparison.InvariantCultureIgnoreCase))
        {
            return "Ping.Ding App";
        }

        if (userAgent.Contains("FxiOS", StringComparison.InvariantCultureIgnoreCase) || userAgent.Contains("Firefox", StringComparison.InvariantCultureIgnoreCase))
        {
            return "Firefox";
        }

        if (userAgent.Contains("OPR", StringComparison.InvariantCultureIgnoreCase))
        {
            return "Opera";
        }

        if (userAgent.Contains("CriOS", StringComparison.InvariantCultureIgnoreCase) || userAgent.Contains("Chrome", StringComparison.InvariantCultureIgnoreCase))
        {
            return "Chrome";
        }

        if (userAgent.Contains("Safari", StringComparison.InvariantCultureIgnoreCase))
        {
            return "Safari";
        }

        return "Other";
    }

    /// <summary>
    /// Helper method to get the operating system from the user agent
    /// </summary>
    /// <param name="os">OS part of the user agent</param>
    /// <param name="isMobile">Whether the user agent is from a mobile device</param>
    /// <returns>Guessed operating system</returns>
    private static string GetOperatingSystem(string os, bool isMobile)
    {
        // This is not a good way to do this, but it that does not matter for that much
        // since this is just to show the user something that they can understand
        // when viewing the session list

        if (isMobile)
        {
            if (os.Contains("Android", StringComparison.InvariantCultureIgnoreCase))
            {
                var osIndex = os.IndexOf("Android ", StringComparison.InvariantCultureIgnoreCase);

                if (osIndex == -1)
                    return "Android";

                // Split the string into the version and the device type
                var deviceTypes = os[osIndex..].Split(';');

                var osVersion = deviceTypes[0].Trim();
                var phoneModel = deviceTypes[1].Trim();

                return $"{phoneModel}: {osVersion}";
            }
            else if (os.Contains("iPhone", StringComparison.InvariantCultureIgnoreCase))
            {
                var osIndex = os.IndexOf("iPhone OS ", StringComparison.InvariantCultureIgnoreCase);

                if (osIndex == -1)
                    return "iPhone";

                // Split the string into the version and the device type
                var osVersion = string.Join('.', os[osIndex..].Split(' ')[2].Split('_'));

                return $"iPhone iOS {osVersion}";
            }

            return "Unknown Mobile";
        }

        // Desktop
        if (os.Contains("Windows"))
        {
            if (os.Contains("NT 10.0"))
                return "Windows 10/11";
            else
                return "Windows";
        }

        if (os.Contains("Mac OS X"))
        {
            var osIndex = os.IndexOf("Mac OS X ", StringComparison.InvariantCultureIgnoreCase);

            if (osIndex == -1)
                return "Mac OS X";

            // Version number is after the 'Mac OS X ' part
            var osVersion = string.Join('.', os[osIndex..].Split(' ')[2].Split('_'));

            return $"Mac OS X {osVersion}";
        }

        if (os.Contains("Linux"))
            return "Linux";

        if (os.Contains("CrOS "))
            return "Chrome OS";

        return "Unknown";
    }
}
