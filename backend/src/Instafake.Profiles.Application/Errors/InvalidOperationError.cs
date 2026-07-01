using FluentResults;

namespace Instafake.Profiles.Application.Errors;

public class InvalidOperationError(string message = "Requested operation is not valid for current resource state.") : Error(message)
{
}
