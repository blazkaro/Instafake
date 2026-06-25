using Instafake.Posts.Application.Events;
using Instafake.Posts.Domain.Events;
using Instafake.Posts.Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Events.Handlers;

internal class PostLikeDeletedEventHandler(PostsDbContext dbContext) : INotificationHandler<DomainEventNotification<PostLikeDeletedEvent>>
{
    private readonly PostsDbContext _dbContext = dbContext;

    public async Task Handle(DomainEventNotification<PostLikeDeletedEvent> notification, CancellationToken cancellationToken)
    {
        await _dbContext.Posts
            .Where(post => post.Id == notification.Event.PostId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(p => p.LikesCount, p => p.LikesCount - 1),
            cancellationToken);
    }
}
