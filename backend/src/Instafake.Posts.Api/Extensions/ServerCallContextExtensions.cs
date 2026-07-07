using Grpc.Core;
using System.Security.Authentication;

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
            return context.GetHttpContext().GetAccessTokenSubject();
        }
    }
}
