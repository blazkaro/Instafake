namespace Instafake.Posts.Domain.Entities;

public class User
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string AvatarUrl { get; init; }
}
