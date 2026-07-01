using Instafake.Profiles.Domain.ValueObjects;

namespace Instafake.Profiles.Domain.Entities;

public class Profile : DomainEntity
{
    public string ProfileId => User.Id;
    public string ProfileName => User.Name;
    public Uri ProfileAvatarUrl => User.AvatarUrl;
    public User User { get; set; }
    public int PostsCount { get; set; }
    public int FollowersCount { get; set; }
    public string Description { get; set; }
}
