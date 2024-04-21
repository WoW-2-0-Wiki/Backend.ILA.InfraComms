using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using N2.MediatR.Streams.StreamRequests;

var services = new ServiceCollection();

services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

var serviceProvider = services.BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();

// Get counted numbers for 5 seconds
Console.WriteLine("Counted numbers:");
await foreach (var number in mediator.CreateStream(new CounterStreamRequest(), new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token))
    Console.WriteLine(number);

// Get random numbers for 5 seconds
Console.WriteLine("Random numbers:");
await foreach(var number in mediator.CreateStream(new RandomNumberStreamRequest(10, 5000), new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token))
    Console.WriteLine(number);

Console.ReadLine();
