using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

internal class PostLikeWriteRepository(PostsDbContext dbContext) : WriteRepositoryBase<Infrastructure.Entities.PostLike, PostsDbContext>(dbContext), IWriteRepository<Domain.Entities.PostLike>
{
    public async Task DeleteAsync(PostLike like, CancellationToken cancellationToken = default) => await BatchDeleteAsync(p => p.PostId == like.PostId && p.UserId == like.UserId, cancellationToken);
    public async Task SaveAsync(PostLike like, CancellationToken cancellationToken = default) => await SaveAsync(like.ToEntity(), cancellationToken);
    public async Task UpdateAsync(PostLike like, CancellationToken cancellationToken = default) => await UpdateAsync(like.ToEntity(), cancellationToken);
}
