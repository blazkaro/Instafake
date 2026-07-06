namespace Instafake.Posts.Domain.Entities;

public class PostLike : DomainEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}
