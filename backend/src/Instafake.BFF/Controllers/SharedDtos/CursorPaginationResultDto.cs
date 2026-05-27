namespace Instafake.BFF.Controllers.SharedDtos;

public class CursorPaginationResultDto<TResult>
{
    public TResult Result { get; set; }
    public CursorPaginationDto? NextCursor { get; set; }
}
