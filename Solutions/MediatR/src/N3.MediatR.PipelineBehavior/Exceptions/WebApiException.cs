using System.Net;

namespace N3.MediatR.PipelineBehavior.Exceptions;

public class WebApiException : Exception
{
    public HttpStatusCode StatusCode { get; set;  } = HttpStatusCode.InternalServerError;

    public WebApiException(string message) : base(message)
    {
    }

    public WebApiException(string message, Exception inner) : base(message, inner)
    {
    }

    public WebApiException(Exception inner) : base(inner.Message, inner)
    {
    }
}