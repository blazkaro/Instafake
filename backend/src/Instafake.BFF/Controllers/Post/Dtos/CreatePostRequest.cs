namespace Instafake.BFF.Controllers.Post.Dtos;

public class CreatePostRequest
{
    public string Description { get; set; }
    public string[] Tags { get; set; }
    public string[] MultimediaUrls { get; set; }
}
