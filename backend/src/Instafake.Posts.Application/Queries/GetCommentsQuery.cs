using Instafake.Posts.Application.Queries.Dtos;
using Instafake.Posts.Application.Queries.Pagination;
using MediatR;

namespace Instafake.Posts.Application.Queries;

public record GetCommentsQuery(Guid PostId, CursorPagination? Cursor) : IRequest<IReadOnlyList<PostCommentDto>>
{
}
