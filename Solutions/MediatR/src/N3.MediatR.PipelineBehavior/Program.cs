using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using N3.MediatR.PipelineBehavior.Requests;

var services = new ServiceCollection();
services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
services.AddLogging(config => config.AddConsole());

var serviceProvider = services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IMediator>();

await mediator.Send(new PingRequest());

Console.ReadLine();