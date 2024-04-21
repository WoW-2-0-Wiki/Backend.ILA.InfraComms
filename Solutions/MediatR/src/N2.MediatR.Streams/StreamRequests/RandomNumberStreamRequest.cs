using MediatR;

namespace N2.MediatR.Streams.StreamRequests;

public record RandomNumberStreamRequest(int Min, int Max) : IStreamRequest<int>;
