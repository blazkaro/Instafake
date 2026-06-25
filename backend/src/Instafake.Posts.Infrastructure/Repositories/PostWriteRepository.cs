using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

internal class PostWriteRepository(PostsDbContext dbContext) : WriteRepositoryBase<Infrastructure.Entities.Post, PostsDbContext>(dbContext), IWriteRepository<Domain.Entities.Post>
{
    public Task DeleteAsync(Post post, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public async Task SaveAsync(Post post, CancellationToken cancellationToken = default) => await SaveAsync(post.ToEntity(), cancellationToken);
    public async Task UpdateAsync(Post post, CancellationToken cancellationToken = default) => await UpdateAsync(post.ToEntity(), cancellationToken);
}
