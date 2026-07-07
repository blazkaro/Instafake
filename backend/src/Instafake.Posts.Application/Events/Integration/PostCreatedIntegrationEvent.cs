using Wolverine.Attributes;

namespace Instafake.Posts.Application.Events.Integration;

[MessageIdentity("post.created")]
public record PostCreatedIntegrationEvent(Guid PostId, string AuthorId)
{
}
