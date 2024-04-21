## Pipeline Behavior

Pipeline behaviors are a way to add cross-cutting concerns to your request/response pipeline. They are executed before and after the request is handled by the handler. They can be used to add logging, validation, caching, and more.

These often called `pipeline behavior`, `behavior` or `processors`

### Exception handler pipeline

This pipeline behavior is used to handle exceptions that occur during the request processing. It is executed after the request is handled by the handler. It can be used to log exceptions, send notifications, and more.

```C#
public class ExceptionHandlerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<ExceptionHandlerPipelineBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlerPipelineBehavior(ILogger<ExceptionHandlerPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request {Request}", request);
            throw;
        }
    }
}
```

### Exception action behavior

```C#

```

### Exception handler and exception action

Rules for exception handler and exception action

- all actions will be executed for the exception, but only one handler can handle it
- exception will be re-thrown after each action completion, but handler can handle and return result
- actions process faster, because handlers work with several `try / catch` blocks
- both support overriding and ordering

### Handler and Actions priority

These are the order of handler or action priorities, if both actions satisfy the priority criteria, both considered as equal

- assembly relevance - whether handler or action is in the assembly same as the request
- namespace relevance - within same assembly, whether handler or action is in the same namespace as the request
- location specification - whether handler or action has namespace with the part of request's namespace 