using Instafake.Notifications.Dtos;
using Wolverine.Attributes;

namespace Instafake.Notifications.Events;

[MessageIdentity("post.notification")]
public record PostNotification(Guid PostId, PostAuthorDto Author, List<string> FollowerIds)
{
}
