using Instafake.Posts.Domain.Entities;

namespace Instafake.Posts.Application.Repositories;

public interface ICommentRepository
{
    Task<Comment> SaveAsync(Comment comment, CancellationToken cancellationToken = default);
    Task<Comment> UpdateAsync(Comment comment, CancellationToken cancellationToken = default);
}
