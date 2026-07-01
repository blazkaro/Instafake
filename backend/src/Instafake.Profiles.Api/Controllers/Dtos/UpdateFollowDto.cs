using System.ComponentModel.DataAnnotations;

namespace Instafake.Profiles.Api.Controllers.Dtos;

public class UpdateFollowDto
{
    [Required]
    public string? ProfileId { get; set; }

    [Required]
    public bool? Follow { get; set; }
}
