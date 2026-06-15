namespace Instafake.Posts.Api.Controllers.Dtos;

public class UpdateLikeDto
{
    public Guid PostId { get; set; }
    public bool Like { get; set; }
}
