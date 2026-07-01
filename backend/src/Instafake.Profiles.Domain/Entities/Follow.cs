namespace Instafake.Profiles.Domain.Entities;

public class Follow : DomainEntity
{
    public string ProfileId { get; set; }
    public string FollowerId { get; set; }
}
