using MediatR;
using N1.MediatR.Messages.Requests;

namespace N1.MediatR.Messages.RequestHandlers;

public class LogTimeStampRequestHandler : IRequestHandler<LogTimeStampRequest>
{
    public Task Handle(LogTimeStampRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine(DateTime.Now);
        return Task.CompletedTask;
    }
}