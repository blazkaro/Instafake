namespace Instafake.Profiles.Domain.Events;

public record FollowDeletedEvent(string ProfileId, string FollowerId) : IDomainEvent
{
}
