using MediatR;

namespace Instafake.Posts.Application.Events;

public class AuthorCreatedEvent : INotification
{
    public string EventType { get; set; }
    public string EventId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
