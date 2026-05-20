namespace Instafake.Posts.Domain.Entities;

public class Author
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string AvatarUrl { get; init; }
}
