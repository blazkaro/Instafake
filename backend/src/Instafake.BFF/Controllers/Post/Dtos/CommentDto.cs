namespace Instafake.BFF.Controllers.Post.Dtos;

public record CommentDto(string Id, AuthorDto Author, string Content, DateTime CreatedAt)
{
}
