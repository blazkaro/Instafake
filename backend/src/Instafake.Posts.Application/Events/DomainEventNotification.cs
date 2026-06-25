using Instafake.Posts.Domain.Events;
using MediatR;

namespace Instafake.Posts.Application.Events;

public class DomainEventNotification<TDomainEvent>(TDomainEvent @event) : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent Event { get; set; } = @event;
}
