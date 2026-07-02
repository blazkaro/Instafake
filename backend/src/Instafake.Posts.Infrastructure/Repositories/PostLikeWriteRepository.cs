using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Repositories;

public class PostLikeWriteRepository(PostsDbContext dbContext) : IWriteRepository<Domain.Entities.PostLike>
{
    public async Task DeleteAsync(PostLike like, CancellationToken cancellationToken = default)
    {
        await dbContext.PostLikes
            .Where(p => p.PostId == like.PostId && p.UserId == like.UserId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task InsertAsync(PostLike like, CancellationToken cancellationToken = default)
    {
        dbContext.PostLikes.Add(like.ToEntity());
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(PostLike like, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
