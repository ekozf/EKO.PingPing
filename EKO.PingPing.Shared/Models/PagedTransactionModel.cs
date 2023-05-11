using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Shared.Models;

public sealed class PagedTransactionModel : ExpiringModelBase
{
    public PagedTransactionModel() { }

    public PagedTransactionModel(int page)
    {
        Page = page;
    }

    public List<TransactionModel> Transactions { get; set; } = new();
    public int Page { get; set; }
    public bool HasReachedEnd { get; set; }

    public override ModelTypeEnum GetModelType()
    {
        return ModelTypeEnum.PagedTransaction;
    }
}
