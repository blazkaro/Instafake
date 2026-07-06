namespace Instafake.Profiles.Domain.Entities;

public class Follow : DomainEntity
{
    public Guid ProfileId { get; set; }
    public Guid FollowerId { get; set; }
}
