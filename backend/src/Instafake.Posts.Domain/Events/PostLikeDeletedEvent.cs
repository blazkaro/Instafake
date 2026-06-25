namespace Instafake.Posts.Domain.Events;

public record PostLikeDeletedEvent(Guid PostId, string UserId) : IDomainEvent
{
}
