using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Shared.Models;

public sealed class PageTransactionListModel : ExpiringModelBase
{
    public List<PagedTransactionModel> PagedTransactions { get; set; } = new List<PagedTransactionModel>();

    public override ModelTypeEnum GetModelType() => ModelTypeEnum.PagedTransaction;
}
