using Instafake.Posts.Application.Events;
using Instafake.Posts.Domain.Events;
using Instafake.Posts.Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Events.Handlers;

internal class CommentCreatedEventHandler(PostsDbContext dbContext) : INotificationHandler<DomainEventNotification<CommentCreatedEvent>>
{
    private readonly PostsDbContext _dbContext = dbContext;

    public async Task Handle(DomainEventNotification<CommentCreatedEvent> notification, CancellationToken cancellationToken)
    {
        await _dbContext.Posts.Where(post => post.Id == notification.Event.PostId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(post => post.CommentsCount, post => post.CommentsCount + 1),
            cancellationToken);
    }
}
