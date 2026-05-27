using Instafake.BFF.Controllers.SharedDtos;
using Microsoft.AspNetCore.Mvc;

namespace Instafake.BFF.Controllers.Post.Dtos;

public class GetCommentsRequest
{
    [FromRoute]
    public string PostId { get; set; }
    public CursorPaginationDto? Cursor { get; set; }
}
