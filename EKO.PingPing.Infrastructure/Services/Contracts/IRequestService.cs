using EKO.PingPing.Shared.Responses;

namespace EKO.PingPing.Infrastructure.Services.Contracts;

/// <summary>
/// Request service interface for making requests to the PingPing website.
/// </summary>
public interface IRequestService
{
    /// <summary>
    /// Log in a user and return the response.
    /// </summary>
    /// <param name="userName">UserName to log in with</param>
    /// <param name="password">Password to use</param>
    /// <returns><see cref="LoginResponse"/> from the server.</returns>
    public Task<LoginResponse> LoginUser(string userName, string password);

    /// <summary>
    /// Get the user purse from the server.
    /// </summary>
    /// <param name="cookie">Authentication cookie of the logged in user</param>
    /// <returns><see cref="PageResponse"/> with the data of the user's purse</returns>
    public Task<PageResponse> GetUserPurse(string cookie);

    /// <summary>
    /// Logs out the user with the given cookie.
    /// </summary>
    /// <param name="cookie">Authentication cookie of the logged in user</param>
    /// <returns>true if the user logged out successfully, otherwise false.</returns>
    public Task<bool> LogOutUser(string cookie);

    /// <summary>
    /// Get the user transactions from the server.
    /// </summary>
    /// <param name="cookie">Authentication cookie of the logged in user</param>
    /// <param name="page">Transaction page</param>
    /// <returns><see cref="PageResponse"/> from the server.</returns>
    public Task<PageResponse> GetTransactions(string cookie, int page = 0);
}
