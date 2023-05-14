using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Shared.Models;

public sealed class SessionsModelList : ExpiringModelBase
{
    public List<SessionsModel> Sessions { get; set; } = new();

    public override ModelTypeEnum GetModelType() => ModelTypeEnum.PagedSessions;
}
