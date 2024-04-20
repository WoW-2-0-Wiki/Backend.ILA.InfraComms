using MediatR;

namespace N1.MediatR.Messages.Requests;

public class PingRequest : IRequest<string>;
