using Grpc.Core;
using System.Security.Authentication;
using System.Security.Claims;

namespace Instafake.Posts.Api.Extensions;

public static class ServerCallContextExtensions
{
    extension(ServerCallContext context)
    {
        /// <summary>
        /// Retrieves access token subject from the gRPC call context.
        /// </summary>
        /// <returns>The subject</returns>
        /// <exception cref="AuthenticationException"></exception>
        public string? GetAccessTokenSubject()
        {
            var httpContext = context.GetHttpContext();
            if (httpContext.User.Identity is null || !httpContext.User.Identity.IsAuthenticated)
                throw new AuthenticationException("Request is not authenticated.");

            var sub = httpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
            return sub;
        }
    }
}
