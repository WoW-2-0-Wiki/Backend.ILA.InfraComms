using MediatR;

namespace N1.MediatR.Streams.StreamRequests;

public record RandomNumberStreamRequest(int Min, int Max) : IStreamRequest<int>;
