namespace Instafake.Posts.Infrastructure.Entities;

public class PostTag
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; }
    public string Tag { get; set; }
}
