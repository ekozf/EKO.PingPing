using EKO.PingPing.Infrastructure.Caching.Contracts;
using EKO.PingPing.Shared.Enums;
using EKO.PingPing.Shared.Models;

namespace EKO.PingPing.Infrastructure.Caching;

/// <summary>
/// Cached store for models
/// </summary>
public sealed class CachedModelRepository : CachedDataStore, ICachedRepository<ExpiringModelBase>
{
    public ExpiringModelBase? Get(ModelTypeEnum type)
    {
        // Get the correct model from the cache
        ExpiringModelBase? model = type switch
        {
            ModelTypeEnum.Purse => _cachedPurse,
            ModelTypeEnum.PagedTransaction => _cachedPageTransaction,
            ModelTypeEnum.Transaction => _cachedTransaction,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

        // If the model is null or expired, remove it and return null
        if (model?.IsExpired != false)
        {
            Remove(type);

            return null;
        }

        // Model is not null and still valid, return it
        return model;
    }

    public void Remove(ModelTypeEnum type)
    {
        // Clear the correct model from the cache
        switch (type)
        {
            case ModelTypeEnum.Purse:
                _cachedPurse = null;
                break;
            case ModelTypeEnum.Transaction:
                _cachedTransaction = null;
                break;
            case ModelTypeEnum.PagedTransaction:
                _cachedPageTransaction = null;
                break;
        }
    }

    public void Set(ExpiringModelBase value)
    {
        // Set the correct model in the cache
        switch (value.GetModelType())
        {
            case ModelTypeEnum.Purse:
                _cachedPurse = (PurseModel)value;
                break;
            case ModelTypeEnum.Transaction:
                _cachedTransaction = (TransactionModel)value;
                break;
            case ModelTypeEnum.PagedTransaction:
                _cachedPageTransaction = (PageTransactionListModel)value;
                break;
        }
    }
}
