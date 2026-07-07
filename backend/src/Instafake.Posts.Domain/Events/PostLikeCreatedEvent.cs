namespace Instafake.Posts.Domain.Events;

public record PostLikeCreatedEvent(Guid PostId, string UserId) : IDomainEvent
{
}
