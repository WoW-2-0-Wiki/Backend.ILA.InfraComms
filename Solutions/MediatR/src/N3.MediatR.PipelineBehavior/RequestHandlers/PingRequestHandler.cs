using MediatR;
using N3.MediatR.PipelineBehavior.Requests;

namespace N3.MediatR.PipelineBehavior.RequestHandlers;

public class PingRequestHandler : IRequestHandler<PingRequest, string>
{
    public Task<string> Handle(PingRequest request, CancellationToken cancellationToken)
    {
        throw new InvalidOperationException("Service unreachable");
    }
}