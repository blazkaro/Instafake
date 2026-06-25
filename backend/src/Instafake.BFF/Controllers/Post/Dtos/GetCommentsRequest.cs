using Instafake.BFF.Controllers.SharedDtos;
using Microsoft.AspNetCore.Mvc;

namespace Instafake.BFF.Controllers.Post.Dtos;

public class GetCommentsRequest
{
    public PaginationDto? Pagination { get; set; }
}
