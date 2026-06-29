using FluentResults;

namespace Instafake.BuildingBlocks.Application.Errors;

public class ResourceNotFound(string message = "Resource was not found.") : Error(message)
{
}
