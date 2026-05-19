using System.Collections.Concurrent;

namespace Instafake.Posts.Domain.Entities;

public class Post
{
    private readonly ConcurrentDictionary<string, byte> _likedBy = [];

    public Guid Id { get; init; } = Guid.NewGuid();
    public required User Author { get; init; }
    public int LikesCount => _likedBy.Count;
    public ConcurrentBag<PostComment> Comments { get; } = [];

    public bool Like(User user) => _likedBy.TryAdd(user.Id, 0);
    public bool Dislike(User user) => _likedBy.TryRemove(user.Id, out _);
    public void AddComment(PostComment comment) => Comments.Add(comment);
}
