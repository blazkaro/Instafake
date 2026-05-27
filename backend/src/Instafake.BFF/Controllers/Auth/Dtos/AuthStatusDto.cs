namespace Instafake.BFF.Controllers.Auth.Dtos;

public record AuthStatusDto(bool IsAuthenticated, string? UserId)
{
}
