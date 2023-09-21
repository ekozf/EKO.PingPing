using EKO.PingPing.Infrastructure.Caching.Contracts;
using EKO.PingPing.Infrastructure.Helpers;
using EKO.PingPing.Infrastructure.Services.Contracts;
using EKO.PingPing.Shared.Enums;
using EKO.PingPing.Shared.Models;
using static EKO.PingPing.Shared.AppConsts;

namespace EKO.PingPing.Infrastructure.Services;

/// <summary>
/// Class that handles everything related to the PingPing service.
/// </summary>
public sealed class PingPingService : IPingPingService
{
    /// <summary>
    /// Request service that handles all the HTTP requests to PingPing.
    /// </summary>
    private readonly IRequestService _request;

    /// <summary>
    /// Cached repository that handles all the caching of the models.
    /// </summary>
    private readonly ICachedRepository<ExpiringModelBase> _cachedRepository;

    public PingPingService(IRequestService request, ICachedRepository<ExpiringModelBase> cachedRepository)
    {
        _request = request;
        _cachedRepository = cachedRepository;
    }

    public async Task DoUserLogin(string userName, string password)
    {
        if (await IsUserLoggedIn())
            return;

        var login = await _request.LoginUser(userName, password);

        if (!PageParser.LoginWasValid(login.Page))
            return;

        // We need to set the cookie in the secure storage, so we can use it later.
        await SecureStorage.Default.SetAsync(COOKIE_KEY, login.Cookie);
    }

    public async Task<bool> IsUserLoggedIn()
    {
        var cookie = await SecureStorage.Default.GetAsync(COOKIE_KEY);

        // If the cookie is null or empty, the user is not logged in.
        return !string.IsNullOrEmpty(cookie);
    }

    public async Task<PurseModel?> GetUserPurse(bool forced = false)
    {
        if (!await IsUserLoggedIn())
            return null;

        // If we are not forced to get the purse, we will try to get it from the cache first.
        if (!forced)
        {
            var cached = _cachedRepository.Get(ModelTypeEnum.Purse);

            if (cached != null)
                return (PurseModel)cached;
        }

        // If we are forced to get the purse, or we don't have it in the cache, we will get it from the website.
        var cookie = await SecureStorage.Default.GetAsync(COOKIE_KEY);

        var purse = await _request.GetUserPurse(cookie);

        var model = PageParser.ParseUserPurse(purse);

        // Save the model in the cache.
        _cachedRepository.Set(model);

        return model;
    }

    public async Task<bool> DoUserLogout()
    {
        if (!await IsUserLoggedIn())
            return true;

        var cookie = await SecureStorage.Default.GetAsync(COOKIE_KEY);

        await _request.LogOutUser(cookie);

        return SecureStorage.Default.Remove(COOKIE_KEY);
    }

    public async Task<PagedTransactionModel?> GetRecentTransactions(bool forced = false)
    {
        return await GetTransactions(0, forced);
    }

    public async Task<PagedTransactionModel?> GetTransactions(int page, bool forced = false)
    {
        if (!await IsUserLoggedIn())
            return null;

        var cached = (_cachedRepository.Get(ModelTypeEnum.PagedTransaction) as PageTransactionListModel)
                    ?? throw new ApplicationException("List of Pages not initialized before accessing data from the cache.");

        // If we are not forced to get the transactions, we will try to get them from the cache first.
        if (!forced)
        {
            var cachedPage = cached.PagedTransactions.Find(x => x.Page == page);

            if (cachedPage != null)
                return cachedPage;
        }
        else
        {
            cached.PagedTransactions.Clear();
        }

        // If we are forced to get the transactions, or we don't have them in the cache, we will get them from the website.
        var cookie = await SecureStorage.Default.GetAsync(COOKIE_KEY);
        var memoryUsed = GC.GetTotalMemory(false) / 1000000.0;
        var transactions = await _request.GetTransactions(cookie, page);
        var memoryUsed1 = GC.GetTotalMemory(false) / 1000000.0;
        var models = PageParser.ParseTransactions(transactions, page);
        var memoryUsed2 = GC.GetTotalMemory(false) / 1000000.0;
        // Save the models in the cache.
        var allCachedPages = (_cachedRepository.Get(ModelTypeEnum.PagedTransaction) as PageTransactionListModel)
                                ?? throw new ApplicationException("List of Pages not initialized before accessing data from the cache.");

        allCachedPages.PagedTransactions.Add(models);

        return models;
    }

    public async Task<SessionsModelList?> GetUserSessions(bool forced = false)
    {
        if (!await IsUserLoggedIn())
            return null;

        // If we are not forced to get the sessions, we will try to get them from the cache first.
        if (!forced)
        {
            var cached = _cachedRepository.Get(ModelTypeEnum.PagedSessions) as SessionsModelList;

            if (cached?.Sessions.Count > 0)
                return cached;
        }

        // If we are forced to get the sessions, or we don't have them in the cache, we will get them from the website.
        var cookie = await SecureStorage.Default.GetAsync(COOKIE_KEY);

        var sessions = await _request.GetAllCurrentSessions(cookie);

        var model = PageParser.ParseUserSessions(sessions);

        // Save the model in the cache.
        _cachedRepository.Set(model);

        return model;
    }

    public async Task<bool> LogoutSession(string sessionId)
    {
        if (!await IsUserLoggedIn())
            return false;

        var cookie = await SecureStorage.Default.GetAsync(COOKIE_KEY);

        return await _request.LogoutSession(cookie, sessionId);
    }
}
