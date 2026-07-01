using Instafake.Profiles.Application.Queries;
using Instafake.Profiles.Application.Queries.Dtos;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Instafake.Profiles.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProfilesController(IMessageBus bus) : ControllerBase
{
    private readonly IMessageBus _bus = bus;

    [HttpGet("{profileName}")]
    public async Task<IActionResult> GetProfile(string profileName, CancellationToken cancellationToken)
    {
        var profile = await _bus.InvokeAsync<ProfileDto>(new GetProfileQuery(profileName), cancellationToken);
        return Ok(profile);
    }
}
