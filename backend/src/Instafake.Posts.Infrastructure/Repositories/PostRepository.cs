using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

internal class PostRepository(PostsDbContext dbContext) : RepositoryBase<Infrastructure.Entities.Post, PostsDbContext>(dbContext), IPostRepository
{
    public async Task<Post> SaveAsync(Post post, CancellationToken cancellationToken = default)
    {
        await SaveAsync(post.ToEntity(), cancellationToken);
        return post;
    }

    public async Task<Post> UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        await UpdateAsync(post.ToEntity(), cancellationToken);
        return post;
    }
}
