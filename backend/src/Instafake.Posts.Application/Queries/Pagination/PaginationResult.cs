namespace Instafake.Posts.Application.Queries.Pagination;

public class PaginationResult<TResult>
{
    public ICollection<TResult> Items { get; set; }
    public int PageSize { get; set; }
    public CursorPagination? NextCursor { get; set; }
}
