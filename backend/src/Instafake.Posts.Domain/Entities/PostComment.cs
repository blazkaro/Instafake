namespace Instafake.Posts.Domain.Entities;

public class PostComment
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid PostId { get; set; }
    public required Guid AuthorId { get; init; }
    public required string Content { get; init; }
    public DateTime CreatedAt { get; init; }
}
