using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Shared.Models;

public sealed class TransactionModel : ExpiringModelBase
{
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }

    public override ModelTypeEnum GetModelType() => ModelTypeEnum.Transaction;
}
