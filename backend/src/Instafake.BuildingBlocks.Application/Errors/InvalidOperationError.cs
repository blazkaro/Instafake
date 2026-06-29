using FluentResults;

namespace Instafake.BuildingBlocks.Application.Errors;

public class InvalidOperationError(string message = "Requested operation is not valid for current resource state.") : Error(message)
{
}
