using MediatR;
using N1.MediatR.Messages.Requests;

namespace N1.MediatR.Messages.RequestHandlers;

public class PingRequestHandler : IRequestHandler<PingRequest, string>
{
    public Task<string> Handle(PingRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult("Pong");
    }
}