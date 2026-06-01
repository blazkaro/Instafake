namespace Instafake.IdentityEventsIngress.Events;

public interface IEventPublisher<in TEvent>
    where TEvent : EventBase, new()
{
    Task PublishAsync(TEvent ev, CancellationToken cancellationToken = default);
}
