using Instafake.IdentityEventsIngress.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instafake.IdentityEventsIngress.Controllers;

[Route("auth0")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class Auth0EventsController(IEventPublisher<UserCreatedEvent> userCreatedPublisher) : ControllerBase
{
    private readonly IEventPublisher<UserCreatedEvent> _userCreatedPublisher = userCreatedPublisher;

    [HttpPost]
    public async Task<IActionResult> HandleAsync([FromBody] Auth0UserCreatedEventDto ev, CancellationToken cancellationToken)
    {
        var identityEvent = new UserCreatedEvent
        {
            Id = ev.Id,
            Type = IdentityEvents.UserCreated,
            UserId = ev.Data.Object.UserId,
            UserName = ev.Data.Object.Nickname,
            AvatarUrl = ev.Data.Object.Picture,
            CreatedAt = ev.Data.Object.CreatedAt
        };

        await _userCreatedPublisher.PublishAsync(identityEvent, cancellationToken);
        return Accepted();
    }
}
