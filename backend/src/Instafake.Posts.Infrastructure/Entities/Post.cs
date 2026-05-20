namespace Instafake.Posts.Infrastructure.Entities;

internal class Post
{
    public Guid Id { get; set; }
    public string AuthorId { get; set; }
    public User Author { get; set; }
    public List<string> MultimediaUrls { get; set; } = [];
    public string Description { get; set; }
    public List<PostTag> Tags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public ICollection<User> LikedBy { get; set; } = [];
    public ICollection<PostComment> Comments { get; set; } = [];
}
