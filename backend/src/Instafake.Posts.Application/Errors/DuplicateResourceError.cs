using FluentResults;

namespace Instafake.Posts.Application.Errors;

public class DuplicateResourceError(string message = "Resource already exists.") : Error(message)
{
}
