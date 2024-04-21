using System.Runtime.CompilerServices;
using MediatR;
using N2.MediatR.Streams.StreamRequests;

namespace N2.MediatR.Streams.StreamRequestHandlers;

public class CounterStreamRequestHandler : IStreamRequestHandler<CounterStreamRequest, int>
{
    public async IAsyncEnumerable<int> Handle(CounterStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var counter = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(1, cancellationToken);
            }
            catch (TaskCanceledException e)
            {
            }

            yield return counter++;
        }
    }
}