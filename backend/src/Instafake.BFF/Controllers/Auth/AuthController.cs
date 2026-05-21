using Auth0.AspNetCore.Authentication;
using Instafake.BFF.Config;
using Instafake.BFF.Controllers.Dtos;
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
    public async Task HandleSignInAsync(string returnUrl = "/")
    {
        if (!returnUrl.StartsWith('/'))
            returnUrl = "/"; // if path isn't local ignore it to prevent open redirection attacks

        var redirectUri = $"{_frontendConfig.Value.Uri.ToString().TrimEnd('/')}{returnUrl}";
        var authProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(redirectUri)
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authProperties);
    }

    [HttpGet("user")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var userName = User.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value ?? User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;
        var avatarUrl = User.Claims.FirstOrDefault(c => c.Type == "picture")!.Value;

        return Ok(new UserInfoDto(id, userName, avatarUrl));
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetAuthenticationStatusAsync()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            return Ok(new AuthStatusDto(true, id));
        }

        return Ok(new AuthStatusDto(false, null));
    }
}
