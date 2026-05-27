using Instafake.BFF.Controllers.SharedDtos;

namespace Instafake.BFF.Controllers.Post.Dtos;

public record GetPostsResponse(List<PostDto> Posts, PaginationResultDto? Pagination)
{
}
