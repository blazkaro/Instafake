using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

internal class CommentWriteRepository(PostsDbContext dbContext) : WriteRepositoryBase<Infrastructure.Entities.PostComment, PostsDbContext>(dbContext), IWriteRepository<Domain.Entities.Comment>
{
    public async Task SaveAsync(Comment comment, CancellationToken cancellationToken = default) => await SaveAsync(comment.ToEntity(), cancellationToken);
    public async Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default) => await UpdateAsync(comment.ToEntity(), cancellationToken);
}
