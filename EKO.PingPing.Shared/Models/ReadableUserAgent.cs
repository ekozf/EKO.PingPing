namespace EKO.PingPing.Shared.Models;

public sealed class ReadableUserAgent
{
    public string Browser { get; set; } = "None";
    public string Os { get; set; } = "None";

    public override string ToString()
    {
        return $"{Browser} - {Os}";
    }
}
