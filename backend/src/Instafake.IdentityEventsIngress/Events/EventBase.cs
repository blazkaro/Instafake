namespace Instafake.IdentityEventsIngress.Events;

public abstract class EventBase
{
    public string EventType { get; set; }
    public string EventId { get; set; }
    public string UserId { get; set; }
}
