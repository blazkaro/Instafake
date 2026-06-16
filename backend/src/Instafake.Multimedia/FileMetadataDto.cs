using System.ComponentModel.DataAnnotations;

namespace Instafake.Multimedia;

public class FileMetadataDto
{
    [Required]
    public string Extension { get; set; }

    [Required]
    public string ContentType { get; set; }

    [Required]
    public int? SizeBytes { get; set; }
}
