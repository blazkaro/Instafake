namespace Instafake.Posts.Application.Queries.Dtos;

public record CommentDto(string Id, AuthorDto Author, string Content, DateTime CreatedAt)
{
}
