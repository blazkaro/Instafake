namespace Instafake.Posts.Domain.Events;

public sealed record CommentCreatedEvent(Guid PostId, Guid CommentId) : IDomainEvent
{
}
