using Instafake.Posts.Application.Events.Integration;
using Instafake.Posts.Domain.Events;

namespace Instafake.Posts.Application.Events;

public class PostCreatedEventHandler
{
    public PostCreatedIntegrationEvent Handle(PostCreatedEvent ev, CancellationToken cancellationToken)
    {
        return new PostCreatedIntegrationEvent(ev.PostId, ev.AuthorId);
    }
}
