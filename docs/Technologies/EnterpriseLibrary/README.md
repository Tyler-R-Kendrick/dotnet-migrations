# Migrating Microsoft Enterprise Library to Modern Alternatives

## Overview

Microsoft Enterprise Library has been a collection of application blocks designed to assist developers with common enterprise development challenges. While it has been beneficial for several aspects of development, many of its features have been made obsolete by modern .NET libraries and patterns. This guide aims to provide steps for migrating from Enterprise Library to these modern alternatives.

[Archive](https://github.com/microsoftarchive/enterprise-library)


## Application Blocks

### Data Access Block

#### Migrate to Microsoft.Extensions.Configuration, EF Core, or Dapper

1. **Analyze the Current Implementation**: Check for existing usage of Data Access Blocks in your application.
2. **Choose a Modern Alternative**: Depending on your needs, choose between EF Core for full ORM capabilities, or Dapper for lightweight data access.
3. **Update Code**: Remove the Enterprise Library references and replace with code that utilizes the chosen library.

**Example Replacement**

*Enterprise Library*

```csharp
Database db = DatabaseFactory.CreateDatabase("MyConnection");
```

*EF Core*

```csharp
var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
optionsBuilder.UseSqlServer(Configuration.GetConnectionString("MyConnection"));
```

### Exception Handling Block

#### Migrate to Serilog or Microsoft.Extensions.Logging

1. **Audit Current Use**: Identify the places in code where Exception Handling Blocks are used.
2. **Choose a Modern Logging Library**: Commonly, Serilog or Microsoft's own logging extensions are good picks.
3. **Replace Code**: Modify the code to use the selected logging library.

**Example Replacement**

*Enterprise Library*

```csharp
ExceptionPolicy.HandleException(ex, "Policy");
```

*Serilog*

```csharp
Log.Error(ex, "An error occurred while processing your request.");
```

### Logging Application Block

#### Migrate to Serilog or Microsoft.Extensions.Logging

The migration steps would be similar to the Exception Handling Block, but you'll replace the logging logic.

**Example Replacement**

*Enterprise Library*

```csharp
Logger.Write("Information", "General");
```

*Microsoft.Extensions.Logging*

```csharp
_logger.LogInformation("Information");
```

### Validation Application Block

#### Migrate to FluentValidation or DataAnnotations

1. **Check Existing Validations**: Catalog all the validations implemented using the Validation Application Block.
2. **Select Validation Library**: FluentValidation is commonly used for complex scenarios, whereas DataAnnotations can be sufficient for simpler needs.
3. **Replace Validations**: Change the existing validation attributes or methods to the chosen library's syntax.

**Example Replacement**

*Enterprise Library*

```csharp
Validation.Validate<Customer>(customer);
```

*FluentValidation*

```csharp
CustomerValidator validator = new CustomerValidator();
ValidationResult results = validator.Validate(customer);
```

### Caching Application Block

#### Migrate to MemoryCache or DistributedCache

1. **Identify Cache Usages**: Locate where caching is done in your application.
2. **Pick a Modern Caching Solution**: MemoryCache is often a suitable replacement for in-memory caching, while DistributedCache serves distributed scenarios.
3. **Update Code**: Remove the Enterprise Library caching code and implement the new caching logic.

**Example Replacement**

*Enterprise Library*

```csharp
CacheManager cache = CacheFactory.GetCacheManager();
```

*MemoryCache*

```csharp
var cache = new MemoryCache(new MemoryCacheOptions());
```

## Additional Resources

- [Microsoft.Extensions.Configuration Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Microsoft.Extensions.Logging Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/en/latest/)
- [MemoryCache Documentation](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache?view=dotnet-plat-ext-5.0)

This document is designed to serve as a comprehensive migration guide for transitioning from Microsoft Enterprise Library to modern libraries and patterns in .NET. It includes steps for each application block, example code, and recommended modern alternatives for feature parity.

### Semantic Logging Application Block

#### Migrate to Serilog with Structured Logging or Microsoft.Extensions.Logging

1. **Identify Current Usages**: Review the areas in which Semantic Logging is employed.
2. **Choose Modern Logging Framework**: Serilog provides rich capabilities for structured logging, as does Microsoft.Extensions.Logging.
3. **Replace Logging Code**: Update your logging implementation to use the chosen library.

**Example Replacement**

*Semantic Logging Application Block*

```csharp
EventSource eventSource = new MyEventSource();
eventSource.Informational(message);
```

*Serilog with Structured Logging*

```csharp
Log.Information("This is an informational message: {@Message}", message);
```

### Caching Application Block

#### Migrate to Microsoft.Extensions.Caching.Memory or Microsoft.Extensions.Caching.Distributed

1. **Identify Cache Usages**: Locate where caching is used.
2. **Pick Modern Solution**: MemoryCache for in-memory and DistributedCache for distributed scenarios.
3. **Update Code**: Implement the new caching strategy.

**Example Replacement**

*Enterprise Library Caching Block*

```csharp
CacheManager cache = CacheFactory.GetCacheManager();
```

*Microsoft.Extensions.Caching.Memory*

```csharp
var cache = new MemoryCache(new MemoryCacheOptions());
```

### Cryptography Application Block

#### Migrate to System.Security.Cryptography or ASP.NET Core Data Protection

1. **Audit Current Implementation**: Identify current cryptographic algorithms and utilities in use.
2. **Choose Modern Crypto Library**: Use System.Security.Cryptography for general-purpose cryptography, or ASP.NET Core Data Protection for web-specific scenarios.
3. **Replace Code**: Implement the chosen cryptographic methods.

**Example Replacement**

*Enterprise Library Cryptography Block*

```csharp
string hash = Cryptographer.CreateHash("provider", input);
```

*System.Security.Cryptography*

```csharp
using (SHA256 sha256 = SHA256.Create())
{
    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
    // Convert byte array to string...
}
```

### Security Application Block

#### Migrate to Microsoft.Identity or IdentityServer

1. **Inspect Current Security Setup**: Identify existing authentication and authorization schemes.
2. **Choose New Identity Management**: Microsoft.Identity for Azure AD or IdentityServer for more customized identity solutions.
3. **Replace Security Code**: Update the authentication and authorization logic.

**Example Replacement**

*Enterprise Library Security Block*

```csharp
bool authorized = SecurityManager.IsAuthorized(user, permission);
```

*Microsoft.Identity with ASP.NET Core*

```csharp
[Authorize(Policy = "YourPolicy")]
public IActionResult YourAction()
{
    // ...
}
```

### Unity Application Block

#### Migrate to Microsoft.Extensions.DependencyInjection

1. **Identify DI Usages**: Find where Unity is being used for dependency injection.
2. **Select Modern DI Library**: Microsoft.Extensions.DependencyInjection is the standard for .NET Core and beyond.
3. **Replace DI Code**: Modify the DI container setup and constructor injections.

**Example Replacement**

*Unity Application Block*

```csharp
IUnityContainer container = new UnityContainer();
container.RegisterType<IMyInterface, MyConcreteClass>();
```

*Microsoft.Extensions.DependencyInjection*

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IMyInterface, MyConcreteClass>();
}
```

## Additional Resources

- [Microsoft.Extensions.Caching Documentation](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory)
- [System.Security.Cryptography Documentation](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography)
- [Microsoft.Identity Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/)
- [Microsoft.Extensions.DependencyInjection Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)

This extended guide should provide you with a pathway to migrate from Microsoft Enterprise Library's application blocks to modern and more scalable alternatives in .NET. The guide covers application blocks that are commonly used, their modern counterparts, example code for replacement, and resources for further learning.

### Transient Fault Handling Application Block

#### Migrate to Polly

1. **Identify Fault Handling Usages**: Locate the places where Transient Fault Handling is used in your application.
2. **Select Polly as Modern Replacement**: Polly is a robust and flexible transient fault-handling library for .NET applications.
3. **Update Code**: Substitute the existing Enterprise Library code with Polly's syntax for retries and transient fault-handling.

**Example Replacement**

*Transient Fault Handling Application Block*

```csharp
var retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(retryStrategy);
retryPolicy.ExecuteAction(() => /* your code */);
```

*Polly*

```csharp
var policy = Policy.Handle<SqlException>().WaitAndRetry(new[] { TimeSpan.FromSeconds(1) });
policy.Execute(() => /* your code */);
```

### Configuration Application Block

#### Migrate to Microsoft.Extensions.Configuration

1. **Examine Existing Configurations**: Identify the existing Configuration Application Block uses.
2. **Choose Microsoft.Extensions.Configuration**: This is the standard configuration system in modern .NET applications.
3. **Update Code**: Replace the old configuration code with the new one.

**Example Replacement**

*Enterprise Library Configuration Block*

```csharp
string setting = ConfigurationManager.AppSettings["SettingKey"];
```

*Microsoft.Extensions.Configuration*

```csharp
string setting = Configuration.GetValue<string>("SettingKey");
```

### Policy Injection Application Block

#### Migrate to Aspect-Oriented Programming (AOP) with PostSharp or Castle DynamicProxy

1. **Locate Policy Injection Usages**: Look for places where you have implemented policy injection.
2. **Choose an AOP Framework**: Both PostSharp and Castle DynamicProxy are commonly used for aspect-oriented programming in .NET.
3. **Update Code**: Migrate the policy injection logic to your chosen AOP library.

**Example Replacement**

*Policy Injection Application Block*

```csharp
PolicyInjector policyInjector = new PolicyInjector();
var myObject = policyInjector.Create<MyClass>();
```

*PostSharp*

```csharp
[MyAspect]
public class MyClass
{
    // ...
}
```

## Additional Resources

- [Polly Documentation](https://github.com/App-vNext/Polly)
- [Microsoft.Extensions.Configuration Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [PostSharp Documentation](https://www.postsharp.net/documentation)
- [Castle DynamicProxy Documentation](https://github.com/castleproject/Core/blob/master/docs/dynamicproxy-intro.md)

This guide should now offer a comprehensive path for migrating from each of the application blocks in Microsoft Enterprise Library to their modern counterparts in .NET. Each section provides a modern alternative, example code for replacement, and steps for each part of the migration process. This information is intended to help you transition smoothly and make your application more maintainable and robust.
