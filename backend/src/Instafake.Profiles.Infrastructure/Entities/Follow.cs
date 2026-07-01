namespace Instafake.Profiles.Infrastructure.Entities;

public class Follow
{
    public Profile Profile { get; set; }
    public string ProfileId { get; set; }
    public string FollowerId { get; set; }
}
