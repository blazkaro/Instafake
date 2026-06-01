namespace Instafake.IdentityEventsIngress.Events;

public abstract class EventBase
{
    public string Type { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
}
