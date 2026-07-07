using Instafake.IdentityEventsIngress.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wolverine;

namespace Instafake.IdentityEventsIngress.Controllers;

[Route("auth0")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class Auth0EventsController(IMessageBus bus) : ControllerBase
{
    private readonly IMessageBus _bus = bus;

    [HttpPost]
    public async Task<IActionResult> HandleAsync([FromBody, Required] Auth0UserCreatedEventDto ev)
    {
        var identityEvent = new UserCreatedEvent
        {
            UserId = ev.Data.Object.UserId,
            UserName = ev.Data.Object.Nickname,
            AvatarUrl = ev.Data.Object.Picture,
            CreatedAt = ev.Data.Object.CreatedAt!.Value
        };

        await _bus.PublishAsync(identityEvent);
        return Accepted();
    }
}
