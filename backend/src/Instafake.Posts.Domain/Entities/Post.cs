namespace Instafake.Posts.Domain.Entities;

public class Post
{
    private readonly HashSet<string> _likedBy = [];
    private readonly List<PostComment> _comments = [];
    private readonly List<string> _multimediaUrls = [];
    private readonly List<string> _tags = [];

    public Guid Id { get; init; } = Guid.NewGuid();
    public required User Author { get; init; }
    public int LikesCount => _likedBy.Count;
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }

    public bool Like(User user) => _likedBy.Add(user.Id);
    public bool Dislike(User user) => _likedBy.Remove(user.Id);

    public void AddComment(User author, string content)
    {
        var comment = new PostComment
        {
            Id = Guid.NewGuid(),
            Author = author,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        _comments.Add(comment);
    }

    public void AddMultimedia(string url)
    {
        _multimediaUrls.Add(url);
    }

    public void AddTag(string tag)
    {
        _tags.Add(tag);
    }
}
