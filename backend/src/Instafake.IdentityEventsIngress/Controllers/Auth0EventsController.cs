using Instafake.IdentityEventsIngress.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Instafake.IdentityEventsIngress.Controllers;

[Route("auth0")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class Auth0EventsController(IEventPublisher<UserCreatedEvent> userCreatedPublisher) : ControllerBase
{
    private readonly IEventPublisher<UserCreatedEvent> _userCreatedPublisher = userCreatedPublisher;

    [HttpPost]
    public async Task<IActionResult> HandleAsync([FromBody, Required] Auth0UserCreatedEventDto ev, CancellationToken cancellationToken)
    {
        var identityEvent = new UserCreatedEvent
        {
            EventId = ev.Id,
            EventType = IdentityEvents.UserCreated,
            UserId = ev.Data.Object.UserId,
            UserName = ev.Data.Object.Nickname,
            AvatarUrl = ev.Data.Object.Picture,
            CreatedAt = ev.Data.Object.CreatedAt!.Value
        };

        await _userCreatedPublisher.PublishAsync(identityEvent, cancellationToken);
        return Accepted();
    }
}
