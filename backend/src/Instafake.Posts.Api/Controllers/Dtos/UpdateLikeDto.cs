using System.ComponentModel.DataAnnotations;

namespace Instafake.Posts.Api.Controllers.Dtos;

public class UpdateLikeDto
{
    [Required]
    public Guid? PostId { get; set; }

    [Required]
    public bool? Like { get; set; }
}
