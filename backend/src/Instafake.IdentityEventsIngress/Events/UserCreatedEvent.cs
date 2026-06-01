namespace Instafake.IdentityEventsIngress.Events;

public class UserCreatedEvent : EventBase
{
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
