namespace OutlookSyncApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Dictionary<string, object>?> GetTokenMessage();
        Task<string> GetToken(string deviceCode);
    }
}
