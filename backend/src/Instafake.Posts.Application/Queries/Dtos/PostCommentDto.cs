namespace Instafake.Posts.Application.Queries.Dtos;

public record PostCommentDto(string Id, AuthorDto Author, string Content, DateTime CreatedAt)
{
}
