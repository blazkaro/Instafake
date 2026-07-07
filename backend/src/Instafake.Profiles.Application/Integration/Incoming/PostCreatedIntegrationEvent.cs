using Wolverine.Attributes;

namespace Instafake.Profiles.Application.Integration.Incoming;

[MessageIdentity("post.created")]
public record PostCreatedIntegrationEvent(Guid PostId, string AuthorId)
{
}
