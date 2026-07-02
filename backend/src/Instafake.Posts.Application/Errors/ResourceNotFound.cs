using FluentResults;

namespace Instafake.Posts.Application.Errors;

public class ResourceNotFound(string message = "Resource was not found.") : Error(message)
{
}
