using Instafake.BuildingBlocks.Domain;
using MediatR;

namespace Instafake.BuildingBlocks.Application.Events.DomainIntegration;

public sealed class DomainEventNotification<TDomainEvent>(TDomainEvent @event) : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent Event { get; set; } = @event;
}