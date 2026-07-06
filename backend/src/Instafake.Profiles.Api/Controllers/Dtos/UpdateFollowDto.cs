using System.ComponentModel.DataAnnotations;

namespace Instafake.Profiles.Api.Controllers.Dtos;

public class UpdateFollowDto
{
    [Required]
    public Guid? ProfileId { get; set; }

    [Required]
    public bool? Follow { get; set; }
}
