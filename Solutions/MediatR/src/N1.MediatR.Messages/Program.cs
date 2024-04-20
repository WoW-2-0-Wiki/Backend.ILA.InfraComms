using MediatR;
using Microsoft.Extensions.DependencyInjection;
using N1.MediatR.Messages.Notifications;
using N1.MediatR.Messages.Requests;

var services = new ServiceCollection();
services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

var serviceProvider = services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IMediator>();

await mediator.Send(new LogTimeStampRequest());

await mediator.Send(new PingRequest());

await mediator.Publish(new UserCreatedNotification { UserId = Guid.NewGuid() });

Console.ReadLine();