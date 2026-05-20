namespace Instafake.Posts.Application.Queries.Pagination;

public class CursorPagination
{
    public DateTime LastItemCreatedAt { get; set; }
    public string Id { get; set; }
    public int PageSize { get; set; }
}
