using Graph.Authentication;
using OutlookSyncApi.Services.Interfaces;

namespace OutlookSyncApi.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly string[] _scopes;
        private readonly string _appId;
        private DeviceCodeAuthProvider _authProvider;

        public AuthenticationService(IConfiguration appConfiguration, IHttpContextAccessor context) 
        {
            _scopes = appConfiguration.GetValue<string>("AzureAd:Scopes").Split(";");
            _appId = appConfiguration.GetValue<string>("AzureAd:ClientId");
            _authProvider = new DeviceCodeAuthProvider(_appId, _scopes, context);
        }

        public async Task<Dictionary<string, object>?> GetTokenMessage()
        {
            return await _authProvider.GetDeviceCode();
        }

        public async Task<string> GetToken(string deviceCode)
        {
            return await _authProvider.GetToken(deviceCode);
        }
    }
}
