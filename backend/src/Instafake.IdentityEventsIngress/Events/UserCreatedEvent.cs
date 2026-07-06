using Wolverine.Attributes;

namespace Instafake.IdentityEventsIngress.Events;

[MessageIdentity(IdentityEvents.UserCreated)]
public class UserCreatedEvent
{
    public Guid UserId => Guid.CreateVersion7();
    public string IdpUserId { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
