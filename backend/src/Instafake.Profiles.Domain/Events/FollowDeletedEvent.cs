namespace Instafake.Profiles.Domain.Events;

public record FollowDeletedEvent(Guid ProfileId, Guid FollowerId) : IDomainEvent
{
}
