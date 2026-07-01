using Wolverine.Attributes;

namespace Instafake.Profiles.Application.Events.Incoming.Users;

[MessageIdentity("user.created")]
public record UserCreatedEvent(string UserId, string UserName, Uri AvatarUrl, DateTime CreatedAt)
{
}
