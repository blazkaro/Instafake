using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

internal class AuthorWriteRepository(PostsDbContext dbContext) : WriteRepositoryBase<Infrastructure.Entities.User, PostsDbContext>(dbContext), IWriteRepository<Domain.Entities.Author>
{
    public async Task SaveAsync(Author author, CancellationToken cancellationToken = default) => await SaveAsync(author.ToEntity(), cancellationToken);
    public async Task UpdateAsync(Author author, CancellationToken cancellationToken = default) => await UpdateAsync(author.ToEntity(), cancellationToken);
}
