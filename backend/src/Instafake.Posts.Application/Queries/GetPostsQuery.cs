using Instafake.Posts.Application.Queries.Dtos;

namespace Instafake.Posts.Application.Queries;

public record GetPostsQuery(string UserId, string? AuthorName, string?[] Tags, PaginationDto? Pagination)
{
}
