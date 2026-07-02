using Wolverine.Attributes;

namespace Instafake.Posts.Application.Events;

[MessageIdentity("user.created")]
public class UserCreatedEvent
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
