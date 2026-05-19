namespace Instafake.Posts.Domain.Entities;

public class PostComment
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required User Author { get; init; }
    public required string Content { get; init; }
}
