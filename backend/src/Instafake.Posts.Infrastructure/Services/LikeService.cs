using Instafake.Posts.Application.Services;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Services;

internal class LikeService(PostsDbContext dbContext) : ILikeService
{
    private readonly PostsDbContext _dbContext = dbContext;

    public async Task Dislike(Guid postId, string userId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var removedCount = await _dbContext.PostLikes
                .Where(postLike => postLike.PostId == postId && postLike.UserId == userId)
                .ExecuteDeleteAsync(cancellationToken);

            if (removedCount == 0)
            {
                await transaction.RollbackAsync(cancellationToken);
                return;
            }

            await _dbContext.Posts
                .Where(post => post.Id == postId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.LikesCount, p => p.LikesCount - 1),
                    cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task Like(Guid postId, string userId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _dbContext.PostLikes.Add(new PostLike { PostId = postId, UserId = userId });
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _dbContext.Posts
                .Where(post => post.Id == postId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.LikesCount, p => p.LikesCount + 1),
                    cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
