using FluentResults;
using Instafake.Posts.Api.Controllers.Dtos;
using Instafake.Posts.Api.Extensions;
using Instafake.Posts.Application.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wolverine;

namespace Instafake.Posts.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class LikesController(IMessageBus bus) : ControllerBase
{
    private readonly IMessageBus _bus = bus;

    [HttpPut]
    public async Task<IActionResult> Update([FromBody, Required] UpdateLikeDto dto, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAccessTokenSubject()!;
        var result = await _bus.InvokeAsync<Result>(new UpdateLikeCommand(dto.Like!.Value, dto.PostId!.Value, userId), cancellationToken);
        if (result.IsFailed)
        {
            return Conflict(result.Errors);
        }

        return Ok();
    }
}
