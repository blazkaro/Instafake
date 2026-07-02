using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;

namespace Instafake.Posts.Application.Events;

public class UserCreatedEventHandler
{
    public async Task Handle(UserCreatedEvent ev, IWriteRepository<Author> repo, CancellationToken cancellationToken)
    {
        var author = new Domain.Entities.Author
        {
            Id = ev.UserId,
            Name = ev.UserName,
            AvatarUrl = ev.AvatarUrl
        };

        await repo.InsertAsync(author, cancellationToken);
    }
}
