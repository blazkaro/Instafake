using Instafake.Posts.Application.Queries.Dtos;
using Instafake.Posts.Application.Queries.Pagination;
using MediatR;

namespace Instafake.Posts.Application.Queries;

public record GetPostsQuery(string UserId, string? AuthorId, string?[] Tags, PaginationDto? Pagination) : IRequest<PaginationResult<IReadOnlyList<PostDto>>>
{
}
