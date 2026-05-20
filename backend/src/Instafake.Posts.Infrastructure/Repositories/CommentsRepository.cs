using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

internal class CommentsRepository(PostsDbContext dbContext) : RepositoryBase<Infrastructure.Entities.PostComment, PostsDbContext>(dbContext), ICommentRepository
{
    public async Task<Comment> SaveAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await SaveAsync(comment.ToEntity(), cancellationToken);
        return comment;
    }

    public async Task<Comment> UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await UpdateAsync(comment.ToEntity(), cancellationToken);
        return comment;
    }
}
