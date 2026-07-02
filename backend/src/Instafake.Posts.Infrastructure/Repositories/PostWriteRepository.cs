using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

public class PostWriteRepository(PostsDbContext dbContext) : IWriteRepository<Domain.Entities.Post>
{
    public Task DeleteAsync(Post post, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task InsertAsync(Post post, CancellationToken cancellationToken = default)
    {
        dbContext.Posts.Add(post.ToEntity());
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(Post post, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
