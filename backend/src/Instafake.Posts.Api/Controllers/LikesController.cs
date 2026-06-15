using Instafake.Posts.Api.Controllers.Dtos;
using Instafake.Posts.Api.Extensions;
using Instafake.Posts.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instafake.Posts.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class LikesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateLikeDto dto, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAccessTokenSubject()!;
        var result = await _mediator.Send(new UpdateLikeCommand(dto.Like, dto.PostId, userId), cancellationToken);
        if (!result)
        {
            return Conflict();
        }

        return Ok();
    }
}
