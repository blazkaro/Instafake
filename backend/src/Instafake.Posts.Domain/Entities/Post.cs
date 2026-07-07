namespace Instafake.Posts.Domain.Entities;

public class Post : DomainEntity
{
    private readonly List<string> _multimediaUrls = [];
    private readonly HashSet<string> _tags = [];

    public Guid Id { get; init; } = Guid.NewGuid();
    public string AuthorId { get; init; }
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyCollection<string> MultimediaUrls => _multimediaUrls.AsReadOnly();
    public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

    public void AddMultimedia(params string[] urls)
    {
        _multimediaUrls.AddRange(urls);
    }

    public void AddTag(params string[] tags)
    {
        foreach (var tag in tags)
        {
            _tags.Add(tag);
        }
    }
}
