# Migrating from System.Configuration to Microsoft.Extensions.Configuration

## Overview

`System.Configuration` is a namespace used in .NET Framework applications for configuration management, usually involving `App.config` or `Web.config` files. The new `Microsoft.Extensions.Configuration` library in ASP.NET Core and .NET provides a more flexible and extensible approach to configuration, supporting different formats like JSON, XML, and environment variables. This document serves as a guideline for migrating from `System.Configuration` to `Microsoft.Extensions.Configuration`.

## Decision Tree for Migration

- Are you using `appSettings` or `connectionStrings` in `Web.config`/`App.config`?
  - Yes: Consider migrating to `appsettings.json` or environment variables in .NET Core/.NET.
  - No: Identify other configuration sections to migrate.

- Are you using custom configuration sections?
  - Yes: Utilize `IConfiguration` sections and custom POCO binding.
  - No: Simpler migration path using key-value pairs or arrays in JSON.

- Are you targeting multiple environments like Dev, Staging, Production?
  - Yes: Use `Microsoft.Extensions.Configuration` to override settings per environment easily.
  - No: Simple settings can still benefit from `Microsoft.Extensions.Configuration` due to its flexibility.

### Before Migration: System.Configuration Usage

Commonly, .NET Framework applications used `App.config` or `Web.config` files, employing custom or built-in configuration sections.

#### System.Configuration Example

```xml
<configuration>
  <appSettings>
    <add key="AppName" value="MyApp" />
  </appSettings>
</configuration>
```

```csharp
string appName = ConfigurationManager.AppSettings["AppName"];
```

### After Migration: Microsoft.Extensions.Configuration

In ASP.NET Core/.NET, you'll typically use `appsettings.json` or other configuration sources.

#### Microsoft.Extensions.Configuration Example

`appsettings.json`

```json
{
  "AppName": "MyApp"
}
```

```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        string appName = Configuration["AppName"];
    }
}
```

## Steps for Migrating

1. **Install Required NuGet Packages**: Make sure to install the `Microsoft.Extensions.Configuration` NuGet packages.
2. **Identify Configuration Sections**: Locate all custom and built-in configuration sections in `App.config` or `Web.config`.
3. **Create `appsettings.json`**: Convert XML-based configurations into JSON in the `appsettings.json` file.
4. **Implement Configuration in Startup**: Utilize `IConfiguration` in your `Startup.cs` or Program to read configurations.
5. **Replace Configuration Access**: Update code that accessed `ConfigurationManager` to use the new `IConfiguration` provider.
6. **Test**: Verify that all configurations are read correctly and all features dependent on it are working as expected.

## Known Issues, Incompatibilities, and Gotchas

- **Strongly Typed Configurations**: `System.Configuration` offered certain strong-typing features that now need to be replicated using `IOptions` or direct POCO mapping.
- **Encrypted Configurations**: Encrypted configuration sections in older apps will have to be decrypted and managed differently in .NET Core/.NET.
- **Config Transformations**: Web.config transformations won't apply; use environment-specific `appsettings.json` files instead.

## Additional Resources

- [Configuration in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0)
- [Replace ConfigurationManager with Microsoft.Extensions.Configuration](https://dev.to/carlos487/replacing-configurationmanager-appsettings-in-net-core-6i8)

This document provides a comprehensive guide for migrating from `System.Configuration` to `Microsoft.Extensions.Configuration`. It includes a decision tree, code examples, and potential issues you may encounter during the migration process.
