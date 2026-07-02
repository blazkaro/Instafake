using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

public class AuthorWriteRepository(PostsDbContext dbContext) : IWriteRepository<Domain.Entities.Author>
{
    public Task DeleteAsync(Author author, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task InsertAsync(Author author, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Add(author.ToEntity());
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Author author, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
