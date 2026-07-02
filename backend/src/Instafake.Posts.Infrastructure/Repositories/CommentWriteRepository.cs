using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Extensions;

namespace Instafake.Posts.Infrastructure.Repositories;

public class CommentWriteRepository(PostsDbContext dbContext) : IWriteRepository<Domain.Entities.Comment>
{
    public Task DeleteAsync(Comment comment, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task InsertAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        dbContext.Comments.Add(comment.ToEntity());
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
