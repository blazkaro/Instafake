namespace Instafake.BFF.Controllers.Dtos;

public record AuthStatusDto(bool IsAuthenticated, string? UserId)
{
}
