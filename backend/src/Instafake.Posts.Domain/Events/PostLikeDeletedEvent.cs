namespace Instafake.Posts.Domain.Events;

public record PostLikeDeletedEvent(Guid PostId, Guid UserId) : IDomainEvent
{
}
