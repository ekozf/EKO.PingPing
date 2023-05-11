using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Shared.Models;

public sealed class PurseModel : ExpiringModelBase
{
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Purse { get; set; } = string.Empty;
    public double Balance { get; set; }

    public override ModelTypeEnum GetModelType() => ModelTypeEnum.Purse;
}
