namespace Instafake.Posts.Infrastructure.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public ICollection<Post> AuthoredPosts { get; set; } = [];
    public ICollection<PostComment> AuthoredComments { get; set; } = [];
    public ICollection<PostLike> Likes { get; set; } = [];
}
