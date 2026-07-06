using Instafake.Profiles.Application.Dtos;
using Wolverine.Attributes;

namespace Instafake.Profiles.Application.Integration.Outgoing;

[MessageIdentity("post.notification")]
public class PostNotification(Guid PostId, PostAuthorDto Author, List<Guid> FollowerIds)
{
}
