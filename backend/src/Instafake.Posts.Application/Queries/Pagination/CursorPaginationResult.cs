namespace Instafake.Posts.Application.Queries.Pagination;

public class CursorPaginationResult<TResult>
{
    public TResult Result { get; set; }
    public CursorPagination? NextCursor { get; set; }
}
