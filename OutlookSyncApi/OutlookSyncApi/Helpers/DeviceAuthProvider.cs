using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Graph.Authentication
{
    public class DeviceCodeAuthProvider : IAuthenticationProvider
    {
        private readonly IPublicClientApplication _msalClient;
        private readonly string[] _scopes;
        private IAccount? _userAccount;
        private readonly string _clientId;
        private readonly IHttpContextAccessor _context;

        public DeviceCodeAuthProvider(string clientId, string[] scopes, IHttpContextAccessor context)
        {
            _scopes = scopes;
            _clientId = clientId;

            _msalClient = PublicClientApplicationBuilder
                .Create(clientId)
                .WithAuthority(AadAuthorityAudience
                    .AzureAdAndPersonalMicrosoftAccount, true)
                .Build();
            _context = context;
        }

        public string GetAccessToken()
        {
            var token = _context.HttpContext.Session.GetString("_accessToken");
            return token;
        }

        public async Task<Dictionary<string, object>?> GetDeviceCode()
        {
            var httpClient = new HttpClient();

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("scope", string.Join("%20", _scopes))
            });

            var response = await httpClient.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/devicecode", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetToken(string deviceCode)
        {
            try
            {
                var tokenUrl = $"https://login.microsoftonline.com/common/oauth2/v2.0/token";
                var requestData = new Dictionary<string, string>
                {
                    { "grant_type", "urn:ietf:params:oauth:grant-type:device_code" },
                    { "client_id", _clientId },
                    { "device_code", deviceCode }
                };

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(requestData));
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                    var accessToken = tokenResponse["access_token"];
                    return accessToken.ToString(); // Return the access token
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return "";
            }
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("bearer", GetAccessToken());
        }

        public async Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
        {
            request.Headers.Add("Authorization", "Bearer " + GetAccessToken());
        }
    }
}