using FluentResults;
using Instafake.Profiles.Api.Controllers.Dtos;
using Instafake.Profiles.Api.Extensions;
using Instafake.Profiles.Application.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wolverine;

namespace Instafake.Profiles.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FollowsController(IMessageBus bus) : ControllerBase
{
    private readonly IMessageBus _bus = bus;

    [HttpPut]
    public async Task<IActionResult> UpdateFollow([FromBody, Required] UpdateFollowDto dto, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAccessTokenSubject()!;
        var result = await _bus.InvokeAsync<Result>(new UpdateFollowCommand(dto.ProfileId!.Value, userId.Value, dto.Follow!.Value), cancellationToken);
        if (result.IsFailed)
        {
            return Conflict(result.Errors);
        }

        return Ok();
    }
}
