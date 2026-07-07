namespace Instafake.Profiles.Domain.Events;

public record ProfileCreatedEvent(string ProfileId) : IDomainEvent
{
}
