using Instafake.BFF.Config;
using Instafake.BFF.Constants;
using Instafake.BFF.Controllers.Auth.Requests;
using Instafake.BFF.Controllers.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Instafake.BFF.Controllers.Auth;

[Route("[controller]")]
[ApiController]
public class AuthController(IOptions<FrontendConfig> frontendConfig) : ControllerBase
{
    private readonly IOptions<FrontendConfig> _frontendConfig = frontendConfig;

    [HttpGet("signin")]
    public async Task<IActionResult> HandleSignInAsync([FromQuery] SignInRequest req)
    {
        if (req.RedirectPath is not null && !req.RedirectPath.StartsWith('/'))
            req.RedirectPath = null; // if path isn't local ignore it to prevent open redirection attacks

        var redirectUri = _frontendConfig.Value.Uri + req.RedirectPath;
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, req.Provider);
    }

    [HttpGet($"callback/{SupportedIdp.GitHub}")]
    public async Task<IActionResult> HandleGitHubCallbackAsync()
    {
        // At this point, the idp response has been checked by OpenIddict, so everything is valid
        var result = await HttpContext.AuthenticateAsync(SupportedIdp.GitHub);
        return SignIn(result.Principal!, result.Properties ?? new());
    }

    [HttpGet("user")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

        return Ok(new UserInfoResponse(id, userName, email));
    }
}
