namespace Instafake.BFF.Controllers.Post.Dtos;

public record PostDto(string Id, AuthorDto Author, string Description, List<string> MultimediaUrls, List<string> Tags, DateTime CreatedAt, int LikesCount, bool LikedByUser, int CommentsCount)
{
}
