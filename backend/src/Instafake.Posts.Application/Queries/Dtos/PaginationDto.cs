using Instafake.Posts.Application.Queries.Pagination;

namespace Instafake.Posts.Application.Queries.Dtos;

public class PaginationDto
{
    public int PageSize { get; set; }
    public CursorPagination? Cursor { get; set; }
}
