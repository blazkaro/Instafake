namespace Instafake.Profiles.Infrastructure.Entities;

internal class Follow
{
    public Profile Profile { get; set; }
    public string ProfileId { get; set; }
    public string FollowerId { get; set; }
}
