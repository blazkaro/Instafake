using Wolverine.Attributes;

namespace Instafake.Posts.Application.Events.Integration;

[MessageIdentity("user.created")]
public class UserCreatedIntegrationEvent
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
