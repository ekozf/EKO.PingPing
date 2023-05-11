using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Infrastructure.Caching.Contracts;

/// <summary>
/// Interface for accessing cached data
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICachedRepository<T> where T : class
{
    /// <summary>
    /// Get a cached model of type <see cref="ModelTypeEnum"/>
    /// </summary>
    /// <param name="type">Type <see cref="ModelTypeEnum"/> to get.</param>
    /// <returns>If the model is found and valid, returns the model, otherwise returns null.</returns>
    public T? Get(ModelTypeEnum type);

    /// <summary>
    /// Remove a cached model of type <see cref="ModelTypeEnum"/>
    /// </summary>
    /// <param name="type">Model type of <see cref="ModelTypeEnum"/> to remove.</param>
    public void Remove(ModelTypeEnum type);

    /// <summary>
    /// Set a cached model
    /// </summary>
    /// <param name="value">Model of type <typeparamref name="T"/> to set.</param>
    public void Set(T value);
}
