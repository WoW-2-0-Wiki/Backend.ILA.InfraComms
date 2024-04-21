using System.Runtime.CompilerServices;
using MediatR;
using N2.MediatR.Streams.StreamRequests;

namespace N2.MediatR.Streams.StreamRequestHandlers;

public class RandomNumberStreamRequestHandler : IStreamRequestHandler<RandomNumberStreamRequest, int>
{
    public async IAsyncEnumerable<int> Handle(RandomNumberStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var random = new Random();
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1);
            yield return random.Next(request.Min, request.Max);
        }
    }
}