using FluentResults;

namespace Instafake.Posts.Application.Errors;

public class ConcurrencyError(string message = "Resource was modified by another process.") : Error(message)
{
}
