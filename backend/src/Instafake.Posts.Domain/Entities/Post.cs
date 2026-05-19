namespace Instafake.Posts.Domain.Entities;

public class Post
{
    private readonly HashSet<string> _likedBy = [];

    public Guid Id { get; init; } = Guid.NewGuid();
    public required User Author { get; init; }
    public int LikesCount => _likedBy.Count;
    public List<string> ImageUrls { get; init; } = [];
    public string Description { get; init; }
    public List<string> Tags { get; init; } = [];
    public DateTime CreatedAt { get; init; }
    public List<PostComment> Comments { get; init; } = [];

    public bool Like(User user) => _likedBy.Add(user.Id);
    public bool Dislike(User user) => _likedBy.Remove(user.Id);
}
