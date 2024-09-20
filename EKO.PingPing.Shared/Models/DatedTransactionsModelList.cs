using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Shared.Models;

public sealed class DatedTransactionsModelList : ExpiringModelBase
{
    private List<DatedTransactionsModel> _datedTransactions = new List<DatedTransactionsModel>();

    public List<DatedTransactionsModel> DatedTransactions
    {
        get => _datedTransactions;
        set => _datedTransactions = value.OrderByDescending(x => x.FromDate).ToList();
    }

    public override ModelTypeEnum GetModelType() => ModelTypeEnum.DatedTransaction;
}
