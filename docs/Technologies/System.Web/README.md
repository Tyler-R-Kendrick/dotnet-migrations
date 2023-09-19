# Migrating from System.Web Assemblies to ASP.NET Core

## Overview

Migrating away from `System.Web` assemblies involves significant changes, as these assemblies are integral to ASP.NET Web Forms and older ASP.NET MVC applications. ASP.NET Core is a modern, performance-optimized successor that allows for cross-platform web applications. See the [ASP.NET Core Roadmap](https://github.com/dotnet/aspnetcore/wiki/Roadmap) for more information on future improvements.

## Decision Tree for Migration

- Is your application using Web Forms?
  - Yes: Consider moving to ASP.NET Core MVC or Blazor.
  - No: Determine which `System.Web` features you're using and identify their ASP.NET Core counterparts.

- Are you using `HttpHandlers` or `HttpModules`?
  - Yes: ASP.NET Core offers Middleware as an alternative.
  - No: Continue to the next decision.

- Is your application relying on `Web.config` for configuration?
  - Yes: ASP.NET Core uses a new configuration system you will need to adapt to.
  - No: Easier transition but still requires manual code changes.

### Before Migration: System.Web Usage

Older ASP.NET applications often relied heavily on `System.Web` namespaces for various web features. 

#### System.Web Example

```csharp
using System.Web;

public class HelloWorldHandler : IHttpHandler 
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Write("Hello World");
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
```

### After Migration: ASP.NET Core

ASP.NET Core provides a modern architecture that you'll migrate to, replacing `System.Web` features.

#### ASP.NET Core Middleware Example

```csharp
public class HelloWorldMiddleware
{
    private readonly RequestDelegate _next;

    public HelloWorldMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await context.Response.WriteAsync("Hello World");
    }
}
```

## Steps for Migrating

1. **Analyze Dependencies**: Identify all the dependencies and functionalities relying on `System.Web`.
2. **Setup ASP.NET Core Project**: Create a new ASP.NET Core project to start migrating features.
3. **Convert Handlers and Modules**: If you're using `HttpHandlers` or `HttpModules`, implement their functionality using Middleware.
4. **Configuration Migration**: Migrate `Web.config` settings to `appsettings.json` and Startup configuration methods.
5. **Test Thoroughly**: Run your tests and verify that the new application behaves as expected.

## Known Issues, Incompatibilities, and Gotchas

- **Web Forms**: Not supported in ASP.NET Core. You will need to migrate to a different technology like MVC or Blazor.
- **State Management**: `Session` and `Application` state work differently in ASP.NET Core.
- **Static Files**: Serving of static files requires setting up Static File Middleware in ASP.NET Core.
- **Authentication**: Forms authentication needs to be migrated to ASP.NET Core Identity or JWT-based authentication.
- **URL Routing**: `Global.asax` for URL routing is replaced by attribute routing in controllers.

## Additional Resources

- [Migrate from ASP.NET to ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/migration/proper-to-2x/?view=aspnetcore-5.0)
- [ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0)

This document serves as a standard reference sheet for migrating from `System.Web` assemblies to ASP.NET Core. It includes a decision-making tree, code examples, known issues, and incompatibilities to watch for during migration.
