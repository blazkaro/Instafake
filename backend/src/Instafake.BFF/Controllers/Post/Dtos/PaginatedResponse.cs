using Instafake.BFF.Controllers.SharedDtos;

namespace Instafake.BFF.Controllers.Post.Dtos;

public record PaginatedResponse<TData>(List<TData> Items, PaginationResultDto? Pagination)
{
}
