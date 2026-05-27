using Microsoft.AspNetCore.Mvc;

namespace Instafake.BFF.Controllers.Post.Dtos;

public class CreateCommenRequest
{
    [FromRoute]
    public string PostId { get; set; }
    public string Content { get; set; }
}
