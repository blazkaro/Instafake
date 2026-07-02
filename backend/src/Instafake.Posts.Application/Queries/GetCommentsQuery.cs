using Instafake.Posts.Application.Queries.Dtos;

namespace Instafake.Posts.Application.Queries;

public record GetCommentsQuery(Guid PostId, PaginationDto? Pagination)
{
}
