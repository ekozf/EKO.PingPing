using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Shared.Models;

public sealed class SessionsModel : ExpiringModelBase
{
    public DateTime LastActiveDate { get; set; }
    public ReadableUserAgent UserAgent { get; set; }
    public string SessionId { get; set; } = string.Empty;

    public override ModelTypeEnum GetModelType() => ModelTypeEnum.Sessions;
}
