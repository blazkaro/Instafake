using FluentResults;

namespace Instafake.BuildingBlocks.Application.Errors;

public class DuplicateResourceError(string message = "Resource already exists.") : Error(message)
{
}
