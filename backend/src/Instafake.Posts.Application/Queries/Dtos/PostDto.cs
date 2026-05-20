namespace Instafake.Posts.Application.Queries.Dtos;

public class PostDto
{
    public string Id { get; set; }
    public AuthorDto Author { get; set; }
    public string Description { get; set; }
    public List<string> MultimediaUrls { get; set; }
    public List<string> Tags { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
}
