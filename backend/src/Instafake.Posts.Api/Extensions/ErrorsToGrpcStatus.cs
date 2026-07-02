using FluentResults;
using Grpc.Core;
using Instafake.Posts.Application.Errors;

namespace Instafake.Posts.Api.Extensions;

public static class ErrorsToGrpcStatus
{
    extension(ResultBase result)
    {
        public Status ToGrpcStatus()
        {
            if (!result.IsFailed)
                throw new ArgumentException("Provided result is not in the failure state");

            return result.Errors[0] switch
            {
                ConcurrencyError er => new Status(StatusCode.Aborted, er.Message),
                DuplicateResourceError er => new Status(StatusCode.AlreadyExists, er.Message),
                InvalidOperationException er => new Status(StatusCode.FailedPrecondition, er.Message),
                ResourceNotFound er => new Status(StatusCode.NotFound, er.Message),
                _ => throw new NotImplementedException("Error to gRPC resolve not implemented for this error type")
            };
        }
    }
}
