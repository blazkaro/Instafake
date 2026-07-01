using Wolverine.Attributes;

namespace Instafake.IdentityEventsIngress.Events;

[MessageIdentity(IdentityEvents.UserCreated)]
public class UserCreatedEvent : EventBase
{
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
