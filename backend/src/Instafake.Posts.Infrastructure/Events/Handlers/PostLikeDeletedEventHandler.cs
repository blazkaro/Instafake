using Instafake.Posts.Domain.Events;
using Instafake.Posts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Events.Handlers;

public class PostLikeDeletedEventHandler
{
    public async Task Handle(PostLikeDeletedEvent ev, PostsDbContext dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Posts
            .Where(post => post.Id == ev.PostId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(p => p.LikesCount, p => p.LikesCount - 1),
            cancellationToken);
    }
}
