using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using MediatR.Pipeline;
using N3.MediatR.PipelineBehavior.Exceptions;

namespace N3.MediatR.PipelineBehavior.PipelineBehaviors.ExceptionBehaviors;

public class WebApiExceptionWrapperMiddleware<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<TResponse>
    where TException : Exception
{
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        state.SetHandled(default!);
        throw new WebApiException(exception)
        {
            StatusCode = exception switch
            {
                ValidationException _ => HttpStatusCode.BadRequest,
                InvalidOperationException _ => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            }
        };
    }
}