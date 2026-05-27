using Instafake.BFF.Controllers.SharedDtos;

namespace Instafake.BFF.Controllers.Post.Dtos;

public record GetCommentsResponse(CommentDto[] Comments, PaginationResultDto? Pagination)
{
}
