## `MediatR` 

## Overview 

This wiki section provides an overview of MediatR, a .NET library that implements the Mediator pattern for infrastructure communication.

Project source [link](https://github.com/jbogard/MediatR) 

### Table of Contents
- [Introduction](#introduction)
- [Key Concepts](#key-concepts)
- [Installation](#installation)
- [Getting Started](#getting-started)
- [Usage Examples](#usage-examples)
- [Best Practices](#best-practices)
- [Common Issues and Solutions](#common-issues-and-solutions)
- [Resources](#resources)
- [FAQs](#faqs)

## Introduction

### What is `MediatR`?

`MediatR` is a .NET library that simplifies implementing the mediator pattern in applications, allowing software components to communicate indirectly through a central mediator. This pattern is useful for managing requests, commands, and notifications across various parts of an application.

- library for infrastructure communication
- implements mediator pattern
- allows components to communicate indirectly through a central mediator

### Why use `MediatR`?

#### Decoupling

Although `service-from-service` decoupling also possible with `MediatR`, applying it for `controller-from-service` decoupling is using it where it should be. 

Using `MediatR` for `controller-from-service` decoupling helps us keep web application endpoints clean from any logic

Example of a controller before using `MediatR`
```C#
[ApiController]
[Route("api/[controller]")]
public class OrganizationsController(
    IMapper mapper,
    IRequestContextProvider requestContextProvider,
    IOrganizationService organizationService) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> Get([FromQuery] OrganizationFilter filter,
        CancellationToken cancellationToken)
    {
        filter.ClientId = requestContextProvider.GetUserId();

        var organizations = await organizationService
            .Get(filter, new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(organization => organization.Products)
            .ProjectTo<OrganizationDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return organizations.Any() ? Ok(organizations) : NoContent();
    }
}
```

Example of a controller after using `MediatR`
```C#
[ApiController]
[Route("api/[controller]")]
public class OrganizationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> Get([FromQuery] OrganizationGetQuery organizationGetQuery, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(organizationGetQuery, cancellationToken);
        return result.Any() ? Ok(result) : NoContent();
    }
}
```

#### Application Requests

While `MediatR` excels in `controller-from-service` decoupling, it also allows for the encapsulation of operation details in a request context that won't be tied to HTTP request. This gives flexibility that is crucial when application logic needs to respond to various types of requests, such as from scheduled jobs, events from an event bus, or other triggers within a system.

```C#
public class OrganizationService(
    IOrganizationRepository organizationRepository,
    IRequestContextProvider requestContextProvider)
    : IOrganizationService
{
    public IQueryable<Organization> Get(
        Expression<Func<Organization, bool>>? predicate, QueryOptions queryOptions = default)
    {
        if (requestContextProvider.GetUserId() == Guid.Empty)
        {
            throw new UnauthorizedAccessException();
        }

        return organizationRepository.Get(predicate, queryOptions);
    }

    public IQueryable<Organization> Get(OrganizationFilter organizationFilter, QueryOptions queryOptions = default)
    {
        var clientId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.ClientId)?.Value;
        
        var organizationQuery = organizationRepository
            .Get()
            .ApplyPagination(organizationFilter);

        if (clientId != null)
            organizationQuery = organizationQuery
                .Where(organization => organization.ClientId == Guid.Parse(clientId));

        return organizationQuery;
    }

```

### Where is `MediatR` used?


### How to implement `MediatR`?




Knowledge check

- Is it a good practice to use `MediatR` for `service-from-service` decoupling ?


Resources

https://www.youtube.com/watch?v=yhpTZDavtsY



- **Mediator Pattern**: Explain the mediator design pattern and its application in MediatR.
- **Commands, Queries, and Notifications**: Define and differentiate these types of messages.

## Key Concepts
- **Mediator Pattern**: Explain the mediator design pattern and its application in MediatR.
- **Commands, Queries, and Notifications**: Define and differentiate these types of messages.

## Installation
- **Prerequisites**: List any prerequisites needed to install MediatR.
- **Installation Steps**: Step-by-step guide to installing MediatR in a .NET project.

## Getting Started
- **Setting Up MediatR in a Project**: Basic setup instructions.
- **Configuration**: Essential configuration settings.

## Usage Examples
- **Basic Usage**: Simple example to demonstrate basic MediatR usage.
- **Advanced Usage**: More complex scenarios and how MediatR handles them.

## Best Practices
- **Design Patterns**: Discuss how to best use MediatR with design patterns like CQRS.
- **Performance Optimization**: Tips for enhancing performance.
- **Testing**: Guidance on testing MediatR implementations.

## Common Issues and Solutions
- **Troubleshooting**: List frequent issues and their resolutions.
- **Debugging Tips**: Effective strategies for debugging.

## Resources
- **Official Documentation**: Link to MediatR's official documentation.
- **Tutorials and Articles**: Curated list of helpful tutorials and articles.
- **Community Contributions**: Links to repositories, plugins, or extensions related to MediatR.

## Knowledge Check
- **Frequently Asked Questions**: Address common queries about using MediatR.