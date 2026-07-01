using FluentResults;

namespace Instafake.Profiles.Application.Errors;

public class ResourceNotFound(string message = "Resource was not found.") : Error(message)
{
}
