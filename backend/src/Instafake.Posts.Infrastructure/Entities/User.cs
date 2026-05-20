namespace Instafake.Posts.Infrastructure.Entities;

internal class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public ICollection<Post> AuthoredPosts { get; set; } = [];
    public ICollection<PostComment> AuthoredComments { get; set; } = [];
    public ICollection<Post> LikedPosts { get; set; } = [];
}
