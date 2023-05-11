using EKO.PingPing.Shared.Enums;

namespace EKO.PingPing.Shared.Models;

/// <summary>
/// Class that represents a data model that expires after a certain amount of time.
/// </summary>
public abstract class ExpiringModelBase
{
    /// <summary>
    /// DateTime of when the model was created.
    /// </summary>
    private readonly DateTime _addedDateTime = DateTime.UtcNow;

    /// <summary>
    /// TimeSpan of how long the model is valid.
    /// </summary>
    private readonly TimeSpan _expirationTimeSpan = TimeSpan.FromHours(1);

    /// <summary>
    /// Property that returns whether the model is expired or not.
    /// </summary>
    public bool IsExpired
    {
        get => DateTime.UtcNow > _addedDateTime + _expirationTimeSpan;
    }

    /// <summary>
    /// Property that returns the unique identifier of the model.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Method that returns the type of the model.
    /// </summary>
    /// <returns>A <see cref="ModelTypeEnum"/> option</returns>
    public abstract ModelTypeEnum GetModelType();
}
