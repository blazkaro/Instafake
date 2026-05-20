namespace Instafake.Posts.Domain.Entities;

public class Post
{
    private readonly List<string> _multimediaUrls = [];
    private readonly List<string> _tags = [];

    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid AuthorId { get; init; }
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyCollection<string> MultimediaUrls => _multimediaUrls.AsReadOnly();
    public IReadOnlyCollection<string> Tags => _multimediaUrls.AsReadOnly();

    public void AddMultimedia(params string[] url)
    {
        _multimediaUrls.AddRange(url);
    }

    public void AddTag(params string[] tag)
    {
        _tags.AddRange(tag);
    }
}
