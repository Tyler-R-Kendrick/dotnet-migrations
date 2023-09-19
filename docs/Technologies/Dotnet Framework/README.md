# Migrating from .NET Framework to .NET 8

[Upgrade to Core](https://learn.microsoft.com/en-us/training/modules/modernize-aspnet-framework-to-core/)

## Overview

Migrating from .NET Framework to the unified .NET 8 platform offers significant benefits, such as performance improvements, enhanced security, and access to modern language features. Check the official [.NET Roadmap](https://github.com/dotnet/core/blob/main/roadmap.md) for future plans around the .NET ecosystem.

## Decision Tree for Migration

Deciding when and how to migrate requires careful consideration of multiple factors:

- Does your application rely on Windows-only technologies (e.g., Windows Forms, WCF)?
  - Yes: Consider a partial migration strategy.
  - No: A complete migration is possible.
  
- Are you using any deprecated or unsupported APIs/packages?
  - Yes: You'll need to update or replace these.
  - No: Continue with migration.

### Using the Upgrade Assistant

Microsoft's [.NET Upgrade Assistant](https://docs.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-overview) is a CLI tool designed to automate many of the steps involved in upgrading .NET Framework projects to .NET 8.

#### Installation

To install the Upgrade Assistant, run:

```bash
dotnet tool install -g upgrade-assistant
```

#### Usage

Execute the assistant by pointing it to your solution or project:

```bash
upgrade-assistant upgrade <Path to Solution/Project File>
```

The Upgrade Assistant will guide you through the upgrade process, making suggestions and performing some tasks automatically.

## Pre-migration Steps

1. **Backup**: Always make a backup of your project before beginning the migration process.
2. **Analyze Dependencies**: Review third-party libraries and NuGet packages for compatibility with .NET 8.
3. **Health Check**: Use tools like [ApiPort](https://github.com/microsoft/dotnet-apiport) to analyze your .NET Framework application for compatibility.

## Migration Steps

1. **Initiate the Upgrade Assistant**: Follow the steps above to run the Upgrade Assistant on your solution or project file.
2. **Manual Intervention**: The Upgrade Assistant will handle many issues, but manual code changes are usually necessary.
3. **Update Build and Deployment Pipelines**: If you have CI/CD pipelines, update them to use .NET 8.
4. **Compile and Test**: Compile your updated project and conduct a thorough round of testing to ensure functionality.

## Post-migration Modernizations

1. **Performance Tuning**: Utilize the performance benefits offered by .NET 8.
2. **New Features**: Adapt new .NET 8-specific language features and APIs.
3. **Package Updates**: Update any third-party packages to versions specifically targeted for .NET 8.

## Known Issues, Incompatibilities, and Gotchas

- **WCF**: If your application uses Windows Communication Foundation, you'll need to migrate to gRPC or another supported service.
- **Web Forms**: Not supported in .NET 8, consider migrating to Blazor or ASP.NET Core MVC.
- **Entity Framework**: EF 6 needs to be updated to EF Core.
- **Windows Services**: Some migration work may be needed to adopt .NET Core's worker services.
- **Assembly Binding**: You may run into assembly binding issues which will require you to update your `app.config` or handle it in code.
  
## Additional Resources

- [.NET 8 Breaking Changes](https://docs.microsoft.com/en-us/dotnet/core/compatibility/)
- [Porting Assistant for .NET](https://aws.amazon.com/porting-assistant/)
- [.NET API Portability Analyzer](https://github.com/microsoft/dotnet-apiport)

This document serves as a standard reference sheet for migrating from .NET Framework to .NET 8. It includes a decision-making tree, outlines the migration process in detail, and provides resources and best practices to anticipate challenges and modernization strategies post-migration.
