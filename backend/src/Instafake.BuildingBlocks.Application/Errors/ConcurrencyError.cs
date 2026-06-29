using FluentResults;

namespace Instafake.BuildingBlocks.Application.Errors;

public class ConcurrencyError(string message = "Resource was modified by another process.") : Error(message)
{
}
