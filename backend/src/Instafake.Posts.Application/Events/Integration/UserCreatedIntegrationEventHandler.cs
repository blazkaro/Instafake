using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;

namespace Instafake.Posts.Application.Events.Integration;

public class UserCreatedIntegrationEventHandler
{
    public async Task Handle(UserCreatedIntegrationEvent ev, IWriteRepository<Author> repo, CancellationToken cancellationToken)
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
