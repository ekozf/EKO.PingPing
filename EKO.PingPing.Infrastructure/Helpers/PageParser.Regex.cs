using System.Text.RegularExpressions;

namespace EKO.PingPing.Infrastructure.Helpers;

public static partial class PageParser
{
    #region Profile Regex

    [GeneratedRegex(pattern: "(?<=data-prepend=\"Username: \" value=\")(.{0,69})(?=\" disabled=\"true\")", RegexOptions.IgnoreCase)]
    private static partial Regex UserNameRegex();

    [GeneratedRegex(pattern: "(?<=data-prepend=\"Email: \" value=\")(.{0,69})(?=\" disabled=\"true\")", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    [GeneratedRegex(pattern: "(?<=data-prepend=\"Name: \" value=\")(.{0,69})(?=\" disabled=\"true\")", RegexOptions.IgnoreCase)]
    private static partial Regex NameRegex();

    [GeneratedRegex(pattern: "(?<=<td class=\"purse\">)(.*)(?=<\\/td>)", RegexOptions.IgnoreCase)]
    private static partial Regex PurseRegex();

    [GeneratedRegex(pattern: "(?<=<td class=\"right\">)(.*)(?=<\\/td>)", RegexOptions.IgnoreCase)]
    private static partial Regex BalanceRegex();

    #endregion
}
