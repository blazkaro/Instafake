namespace Instafake.Profiles.Domain.Events;

public record ProfileCreatedEvent(Guid ProfileId) : IDomainEvent
{
}
