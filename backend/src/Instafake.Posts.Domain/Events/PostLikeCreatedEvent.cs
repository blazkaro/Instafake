namespace Instafake.Posts.Domain.Events;

public record PostLikeCreatedEvent(Guid PostId, Guid UserId) : IDomainEvent
{
}
