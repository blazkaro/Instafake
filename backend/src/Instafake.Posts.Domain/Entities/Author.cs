namespace Instafake.Posts.Domain.Entities;

public class Author : DomainEntity
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string AvatarUrl { get; init; }
}
