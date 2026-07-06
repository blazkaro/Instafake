namespace Instafake.Profiles.Domain.Events;

public record FollowCreatedEvent(Guid ProfileId, Guid FollowerId) : IDomainEvent
{
}
