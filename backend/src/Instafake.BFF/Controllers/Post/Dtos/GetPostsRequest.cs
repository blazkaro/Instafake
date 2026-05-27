using Instafake.BFF.Controllers.SharedDtos;

namespace Instafake.BFF.Controllers.Post.Dtos;

public class GetPostsRequest
{
    public string[]? Tags { get; set; }
    public string? AuthorId { get; set; }
    public PaginationDto? Pagination { get; set; }
}
