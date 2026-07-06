using Wolverine.Attributes;

namespace Instafake.Profiles.Application.Integration.Incoming;

[MessageIdentity("user.created")]
public record UserCreatedIntegrationEvent(Guid UserId, string UserName, Uri AvatarUrl, DateTime CreatedAt)
{
}
