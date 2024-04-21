using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace N3.MediatR.PipelineBehavior.PipelineBehaviors.ExceptionBehaviors;

public class ExceptionLoggingHandler<TRequest, TResponse, TException>(ILogger<ExceptionLoggingHandler<TRequest, TResponse, TException>> logger)
    : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<TResponse>
    where TException : Exception
{
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Request processing failed for request {Request}", typeof(TRequest).Name);
        return Task.CompletedTask;
    }
}