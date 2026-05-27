namespace Instafake.BFF.Controllers.SharedDtos;

public class PaginationResultDto
{
    public int PageSize { get; set; }
    public CursorPaginationDto? NextCursor { get; set; }
}
