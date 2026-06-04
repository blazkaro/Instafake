using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

internal class AuthorRepository(PostsDbContext dbContext) : RepositoryBase<Infrastructure.Entities.User, PostsDbContext>(dbContext), IAuthorRepository
{
    public async Task<Author> SaveAsync(Author author, CancellationToken cancellationToken = default)
    {
        await SaveAsync(author.ToEntity(), cancellationToken);
        return author;
    }

    public async Task<Author> UpdateAsync(Author author, CancellationToken cancellationToken = default)
    {
        await UpdateAsync(author.ToEntity(), cancellationToken);
        return author;
    }
}
