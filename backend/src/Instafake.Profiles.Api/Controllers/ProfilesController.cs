using FluentResults;
using Instafake.Profiles.Api.Extensions;
using Instafake.Profiles.Application.Queries;
using Instafake.Profiles.Application.Queries.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Instafake.Profiles.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfilesController(IMessageBus bus) : ControllerBase
{
    private readonly IMessageBus _bus = bus;

    [HttpGet("{profileName}")]
    public async Task<IActionResult> GetProfile(string profileName, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAccessTokenSubject()!;
        var result = await _bus.InvokeAsync<Result<ProfileDto>>(new GetProfileQuery(userId, profileName), cancellationToken);
        if (result.IsFailed)
            return NotFound();

        return Ok(result.Value);
    }
}
