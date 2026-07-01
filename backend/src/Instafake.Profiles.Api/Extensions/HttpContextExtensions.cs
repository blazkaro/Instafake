using System.Security.Authentication;
using System.Security.Claims;

namespace Instafake.Profiles.Api.Extensions;

public static class HttpContextExtensions
{
    extension(HttpContext httpContext)
    {
        /// <summary>
        /// Retrieves access token subject from the <see cref="HttpContext"/>
        /// </summary>
        /// <returns>The subject</returns>
        /// <exception cref="AuthenticationException"></exception>
        public string? GetAccessTokenSubject()
        {
            if (httpContext.User.Identity is null || !httpContext.User.Identity.IsAuthenticated)
                throw new AuthenticationException("Request is not authenticated.");

            var sub = httpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
            return sub;
        }
    }
}