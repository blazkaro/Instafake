using Grpc.Core;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Authentication;

namespace Instafake.Posts.Api.Extensions;

public static class ServerCallContextExtensions
{
    extension(ServerCallContext context)
    {
        /// <summary>
        /// Retrieves access token subject from the gRPC call context.
        /// </summary>
        /// <returns>The user id</returns>
        /// <exception cref="AuthenticationException"></exception>
        public string? GetAccessTokenSubject()
        {
            var httpContext = context.GetHttpContext();
            if (httpContext.User.Identity?.IsAuthenticated ?? false)
                throw new AuthenticationException("Request is not authenticated.");

            var sub = httpContext.User.Claims.FirstOrDefault(p => p.Type == JwtRegisteredClaimNames.Sub)?.Value;
            return sub;
        }
    }
}
