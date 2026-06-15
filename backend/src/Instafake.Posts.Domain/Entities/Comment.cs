namespace Instafake.Posts.Domain.Entities;

public class Comment : DomainEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid PostId { get; set; }
    public required string AuthorId { get; init; }
    public required string Content { get; init; }
    public DateTime CreatedAt { get; init; }
}
