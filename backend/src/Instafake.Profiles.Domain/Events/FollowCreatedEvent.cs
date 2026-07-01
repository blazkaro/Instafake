namespace Instafake.Profiles.Domain.Events;

public record FollowCreatedEvent(string ProfileId, string FollowerId) : IDomainEvent
{
}
