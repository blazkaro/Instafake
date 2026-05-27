namespace Instafake.Posts.Application.Queries.Pagination;

public class PaginationResult<TResult>
{
    public TResult Result { get; set; }
    public int PageSize { get; set; }
    public CursorPagination? NextCursor { get; set; }
}
