using Instafake.Posts.Domain.Entities;

namespace Instafake.Posts.Application.Repositories;

public interface IAuthorRepository
{
    Task<Author> SaveAsync(Author author, CancellationToken cancellationToken = default);
    Task<Author> UpdateAsync(Author author, CancellationToken cancellationToken = default);
}
