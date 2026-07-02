using Instafake.Posts.Domain.Events;
using Instafake.Posts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Events.Handlers;

public class CommentCreatedEventHandler
{
    public async Task Handle(CommentCreatedEvent ev, PostsDbContext dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Posts.Where(post => post.Id == ev.PostId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(post => post.CommentsCount, post => post.CommentsCount + 1),
            cancellationToken);
    }
}
