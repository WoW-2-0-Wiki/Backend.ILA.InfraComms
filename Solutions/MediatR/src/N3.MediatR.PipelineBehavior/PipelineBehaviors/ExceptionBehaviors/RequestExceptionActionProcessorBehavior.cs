using MediatR.Pipeline;

namespace N3.MediatR.PipelineBehavior.PipelineBehaviors.ExceptionBehaviors;

public class RequestExceptionActionProcessorBehavior<TRequest, TException> : IRequestExceptionAction<TRequest, TException>
    where TRequest : notnull
    where TException : Exception
{
    public Task Execute(TRequest request, TException exception, CancellationToken cancellationToken)
    {
        Console.WriteLine("test a");
        return Task.CompletedTask;
    }
}