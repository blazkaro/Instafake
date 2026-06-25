using Instafake.Posts.Application.Events;
using Instafake.Posts.Domain.Entities;
using MediatR;

namespace Instafake.Posts.Application.Commands.Handlers;

internal abstract class CommandHandlerBase<TDomainEntity>(IPublisher publisher)
    where TDomainEntity : DomainEntity
{
    private readonly IPublisher _publisher = publisher;

    protected async Task DispatchEvents(TDomainEntity entity)
    {
        // Events have to be processed in order so do not think about Task.WhenAll etc
        foreach (var ev in entity.Events)
        {
            // TODO: Avoid costly reflection. 1: Make domain layer dependant on mediatr contracts or 2: Cached compiled expressions
            // TODO: Outbox pattern
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(ev.GetType());

            var notification = (INotification)Activator.CreateInstance(notificationType, ev)!;
            await _publisher.Publish(notification);
        }
    }
}
