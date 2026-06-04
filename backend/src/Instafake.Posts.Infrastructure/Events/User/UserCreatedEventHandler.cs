using Instafake.Posts.Application.Events;
using Instafake.Posts.Application.Repositories;
using MediatR;

namespace Instafake.Posts.Infrastructure.Events.User;

internal class UserCreatedEventHandler(IAuthorRepository repo) : INotificationHandler<AuthorCreatedEvent>
{
    private readonly IAuthorRepository _repo = repo;

    public async Task Handle(AuthorCreatedEvent notification, CancellationToken cancellationToken)
    {
        var author = new Domain.Entities.Author
        {
            Id = notification.EventId,
            Name = notification.UserName,
            AvatarUrl = notification.AvatarUrl
        };

        await _repo.SaveAsync(author, cancellationToken);
    }
}
