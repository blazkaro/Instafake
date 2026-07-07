namespace Instafake.Profiles.Infrastructure.Entities;

public class Profile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public int PostsCount { get; set; }
    public int FollowersCount { get; set; }
    public string Description { get; set; }
    public ProfileCounter ProfileCounter { get; set; }
    public ICollection<Follow> Follows { get; set; }
}
