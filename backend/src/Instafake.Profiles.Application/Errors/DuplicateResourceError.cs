using FluentResults;

namespace Instafake.Profiles.Application.Errors;

public class DuplicateResourceError(string message = "Resource already exists.") : Error(message)
{
}
