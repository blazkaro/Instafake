namespace Instafake.Posts.Infrastructure.Entities;

public class PostComment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
