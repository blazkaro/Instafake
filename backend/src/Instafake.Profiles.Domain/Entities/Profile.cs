namespace Instafake.Profiles.Domain.Entities;

public class Profile : DomainEntity
{
    public User User { get; set; }
    public int PostsCount { get; set; }
    public int FollowersCount { get; set; }
    public string Description { get; set; }
}
