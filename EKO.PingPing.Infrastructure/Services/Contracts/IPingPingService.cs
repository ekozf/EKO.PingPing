using EKO.PingPing.Shared.Models;

namespace EKO.PingPing.Infrastructure.Services.Contracts;

/// <summary>
/// Service that handles all the communication with the PingPing API, storing data, and user login.
/// </summary>
public interface IPingPingService
{
    /// <summary>
    /// Check if the user is logged in.
    /// </summary>
    /// <returns>true if the user is logged in, otherwise false.</returns>
    public Task<bool> IsUserLoggedIn();

    /// <summary>
    /// Log in the user with the given credentials and save them in SecureStorage.
    /// </summary>
    /// <param name="userName">UserName of the user</param>
    /// <param name="password">Password of the user</param>
    public Task DoUserLogin(string userName, string password);

    /// <summary>
    /// Get the user's purse from the API.
    /// </summary>
    /// <param name="forced">Whether to ignore cached results, true forces a request to the API, false allows for data to be retrieved from the cache. Default is false.</param>
    /// <returns>If the user is correctly logged in, returns the <see cref="PurseModel"/> otherwise returns null.</returns>
    public Task<PurseModel?> GetUserPurse(bool forced = false);

    /// <summary>
    /// Log out the user and clear the SecureStorage.
    /// </summary>
    public Task<bool> DoUserLogout();

    /// <summary>
    /// Get the user's recent transactions from the API.
    /// </summary>
    /// <param name="forced">Whether to ignore cached results, true forces a request to the API, false allows for data to be retrieved from the cache. Default is false.</param>
    /// <returns>If the user is correctly logged in, returns an IEnumerable of <see cref="TransactionModel"/> otherwise returns null.</returns>
    public Task<PagedTransactionModel?> GetRecentTransactions(bool forced = false);

    /// <summary>
    /// Get a page from the user's transactions from the API.
    /// </summary>
    /// <param name="page">Transaction page</param>
    /// <param name="forced">Whether to ignore cached results, true forces a request to the API, false allows for data to be retrieved from the cache. Default is false.</param>
    /// <returns>If the user is correctly logged in, returns an IEnumerable of <see cref="TransactionModel"/> otherwise returns null.</returns>
    public Task<PagedTransactionModel?> GetTransactions(int page, bool forced = false);
}
