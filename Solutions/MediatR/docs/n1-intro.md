## `MediatR` 

## Overview 

This wiki section provides an overview of MediatR, a .NET library that implements the Mediator pattern for infrastructure communication.

Project source [link](https://github.com/jbogard/MediatR) 

### Table of Contents
- [What is `MediatR`?](#what-is-mediatr)
- [Why use `MediatR`](#why-use-mediatr)
- [Where is `MediatR` used?](#where-is-mediatr-used)
- [How to use `MediatR`](#how-to-use-mediatr)

## Remaining sections

- Exception Action

## Introduction

### What is `MediatR`?

`MediatR` is a .NET library that simplifies implementing the mediator pattern in applications, allowing software components to communicate indirectly through a central mediator. This pattern is useful for managing requests, commands, and notifications across various parts of an application.

- library for in-process messaging between infrastructure components
- implements mediator pattern
- allows components to communicate indirectly through a central mediator

### Why use `MediatR`?

#### Decoupling

Although `service-from-service` decoupling also possible with `MediatR`, applying it for `controller-from-service` decoupling would be a best fit.

Following is an example of having business logic inside a controller, which mixes exposer logic with business logic.

```C#
[ApiController]
[Route("api/[controller]")]
public class OrganizationsController(
    IMapper mapper,
    IRequestContextProvider requestContextProvider,
    IOrganizationService organizationService) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> Get([FromQuery] OrganizationFilter filter)
    {
        filter.ClientId = requestContextProvider.GetUserId();

        var organizations = await organizationService
            .Get(filter, new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(organization => organization.Products)
            .ProjectTo<OrganizationDto>(mapper.ConfigurationProvider)
            .ToListAsync();

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
    public async ValueTask<IActionResult> Get([FromQuery] OrganizationGetQuery organizationGetQuery)
    {
        var result = await mediator.Send(organizationGetQuery);
        return result.Any() ? Ok(result) : NoContent();
    }
}
```

#### Application Requests

While `MediatR` excels in `controller-from-service` decoupling, it also allows for the encapsulation of operation details in a request context that won't be tied to HTTP request. This gives flexibility that is crucial when application logic needs to respond to various types of requests, such as from scheduled jobs, events from an event bus, or other triggers within a system.

Following is an example of a service that uses HTTP context to execute query. Calling such service for other requests than HTTP request might result in errors most of the cases

```C#
public class OrganizationService(
    IOrganizationRepository organizationRepository,
    IHttpContextAccessor httpContextAccessor)
    : IOrganizationService
{
    public IQueryable<Organization> Get(OrganizationFilter organizationFilter)
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
}
```

Example of a service after using `MediatR` and `MediatR` request handler

```C#
public class OrganizationService(IOrganizationRepository organizationRepository)
    : IOrganizationService
{
    public IQueryable<Organization> Get(OrganizationFilter organizationFilter)
    {
        var organizationQuery = organizationRepository
            .Get()
            .ApplyPagination(organizationFilter);

        if (organizationFilter.ClientId.HasValue)
            organizationQuery = organizationQuery
                .Where(organization => organization.ClientId == organizationFilter.ClientId);

        return organizationQuery;
    }
}

public class OrganizationGetQueryHandler(
    IMapper mapper,
    IOrganizationService organizationService,
    IRequestContextProvider requestContextProvider)
    : IQueryHandler<OrganizationGetQuery, ICollection<OrganizationDto>>
{
    public async Task<ICollection<OrganizationDto>> Handle(OrganizationGetQuery organizationGetQuery,
        CancellationToken cancellationToken)
    {
        organizationGetQuery.Filter.ClientId = requestContextProvider.GetUserId();

        return await organizationService
            .Get(organizationGetQuery.Filter, new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(organization => organization.Products)
            .ProjectTo<OrganizationDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
```

####

### Where is `MediatR` used?

#### In-process messaging

Do not use `MediatR` for processes that takes long time for an ordinary request, `out-of-process` event-messaging solutions are exactly for this kind of problems

## How to use `MediatR`

- [Basics](n2-basics.md)

#### Resources

https://www.youtube.com/watch?v=yhpTZDavtsY