namespace Instafake.BFF.Controllers.SharedDtos;

public class PaginationDto
{
    public int PageSize { get; set; }
    public CursorPaginationDto? Cursor { get; set; }
}
