using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OutlookSyncApi.Services.Interfaces;

namespace OutlookSyncApi.Controllers
{
    [Route("/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IHttpContextAccessor _context;
        private readonly IAuthService _authenticationService;

        public AuthController(IAuthService authService, IHttpContextAccessor context) 
        {
            _authenticationService = authService;
            _context = context;
        }

        [HttpGet("/DeviceCode")]
        public async Task<ActionResult> GetMessage()
        {
            var response = await _authenticationService.GetTokenMessage();
            if (response != null)
                _context.HttpContext.Session.SetString("_deviceCode", response["device_code"].ToString());

            return Ok(response?["message"].ToString());

        }

        [HttpGet("/AccessToken")]
        public async Task<ActionResult> GetToken()
        {
            var deviceCode = _context.HttpContext.Session.GetString("_deviceCode");
            if (deviceCode == null)
            {
                return Unauthorized();
            }

            var token = await _authenticationService.GetToken(deviceCode);

            if (token == null)
                return Unauthorized();

            _context.HttpContext.Session.SetString("_accessToken", token);

            return Ok(token);
        }

    }
}
