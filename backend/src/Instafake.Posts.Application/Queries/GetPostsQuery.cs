using Instafake.Posts.Application.Queries.Dtos;

namespace Instafake.Posts.Application.Queries;

public record GetPostsQuery(Guid UserId, string? AuthorName, string?[] Tags, PaginationDto? Pagination)
{
}
