using EKO.PingPing.Shared.Models;

namespace EKO.PingPing.Infrastructure.Caching;

/// <summary>
/// Wrapper class for cached objects.
/// </summary>
public class CachedDataStore
{
    internal PurseModel? _cachedPurse;
    internal TransactionModel? _cachedTransaction;
    internal PageTransactionListModel? _cachedPageTransaction = new();
    internal SessionsModel? _cachedSession;
    internal SessionsModelList? _cachedPagedSessions = new();
}
