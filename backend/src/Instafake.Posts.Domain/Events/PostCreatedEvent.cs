namespace Instafake.Posts.Domain.Events;

public record PostCreatedEvent(Guid PostId, string AuthorId) : IDomainEvent
{
}
