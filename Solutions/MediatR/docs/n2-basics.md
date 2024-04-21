## MediatR Basics

### Setup

Install the package via NuGet first: Install-Package MediatR

### Registration

Registering by scanning all types from all assemblies
```C#
var assemblies = Assembly.GetExecutingAssembly()
        .GetReferencedAssemblies()
        .Select(Assembly.Load)
        .Append(Assembly.GetExecutingAssembly())
        .ToList();

builder.Services.AddMediatR(conf => { conf.RegisterServicesFromAssemblies(assemblies.ToArray()); });
```

This method registers the known MediatR types:

`IMediator` as transient
`ISender` as transient
`IPublisher` as transient

For all registered assemblies, MediatR will scan following types (behaviors excluded)

IRequestHandler<,> concrete implementations as transient
IRequestHandler<> concrete implementations as transient
INotificationHandler<> concrete implementations as transient
IStreamRequestHandler<> concrete implementations as transient
IRequestExceptionHandler<,,> concrete implementations as transient
IRequestExceptionAction<,>) concrete implementations as transient
Behaviors and pre/post processors must be registered explicitly through the AddXyz methods.

### Messages

`MediatR` dispatches 2 kind of messages

- `Request`/`Response` dispatched to a single handler
- `Notification` messages dispatched to multiple handlers

`Request`/`Response` messages may need a return value or may not

Request that needs a return value
```C#
public class Ping : IRequest<string> { }
```

Request that does not need a return value
```C#
public class DeleteUserCommand: IRequest { }
```

So for that reason, `MediatR` has 2 request types
- IRequest<TResponse> - for requests that need a return value
- IRequest - for requests that do not need a return value

and 2 request handlers for those requests
- IRequestHandler<TRequest, TResponse> - for requests that need a return value
- IRequestHandler<TRequest> - for requests that do not need a return value

### Notifications

Notification messages are dispatched to multiple handlers, used to notify other parts of the system about an event

- `Notification` - notification message interface
- `INotificationHandler<TNotification>` - notification handler interface

Notification
```C#
public class UserRegistered : INotification { }
```

Notification handler
```C#
public class UserRegisteredHandler : INotificationHandler<UserRegistered>
{
    public Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        // Handle the notification
        return Task.CompletedTask;
    }
}
```

### Custom Notification Publishers

### Streams

Stream requests are used to get response in stream

- `IStreamRequest<TResponse>` - stream request interface
- `IStreamRequestHandler<TRequest, TResponse>` - stream request handler interface

Stream request
```C#
public class RandomNumberStreamRequest : IStreamRequest<int> { }
```

Stream request handler
```C#
public class RandomNumberStreamRequestHandler : IStreamRequestHandler<RandomNumberStreamRequest, int>
{
    public async IAsyncEnumerable<int> Handle(RandomNumberStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var random = new Random();
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1);
            yield return random.Next();
        }
    }
}
```

Using stream request
```C#
await foreach(var number in mediator.CreateStream(new RandomNumberStreamRequest(10, 5000), new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token))
    Console.WriteLine(number);
```