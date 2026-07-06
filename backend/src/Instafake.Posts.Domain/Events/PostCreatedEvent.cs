namespace Instafake.Posts.Domain.Events;

public record PostCreatedEvent(Guid PostId, Guid AuthorId) : IDomainEvent
{
}
