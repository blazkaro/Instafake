using Instafake.Profiles.Application.Dtos;
using Wolverine.Attributes;

namespace Instafake.Profiles.Application.Integration.Outgoing;

[MessageIdentity("post.notification")]
public record PostNotification(Guid PostId, PostAuthorDto Author, List<string> FollowerIds)
{
}
