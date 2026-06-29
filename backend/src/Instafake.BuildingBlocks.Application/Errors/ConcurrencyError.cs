using FluentResults;

namespace Instafake.BuildingBlocks.Application.Errors;

public class ConcurrencyError(string message = "The resource was modified by another process.") : Error(message)
{
}
