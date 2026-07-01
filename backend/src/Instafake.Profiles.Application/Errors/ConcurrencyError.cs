using FluentResults;

namespace Instafake.Profiles.Application.Errors;

public class ConcurrencyError(string message = "Resource was modified by another process.") : Error(message)
{
}
