# Migrating from .NET Core to .NET 8

## Overview

With the unification of the .NET ecosystem, migrating to the latest version of .NET (currently .NET 8) provides several benefits, including performance improvements, enhanced productivity, and support for the latest features. The future of .NET is outlined in the [.NET Roadmap](https://github.com/dotnet/core/blob/main/roadmap.md).

## Decision Tree for Migration

Before beginning the migration process, it's crucial to evaluate the needs and constraints of your project.

- Is your application using .NET Core 3.1 or earlier?
  - Yes: Consider moving to .NET 5 before proceeding to .NET 8.
  - No: Proceed to .NET 8 migration.
  
- Are you using any deprecated/unsupported packages or APIs?
  - Yes: Update or replace them.
  - No: Continue with migration.

### Using Upgrade Assistant

Microsoft provides the [.NET Upgrade Assistant](https://docs.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-overview), a tool designed to assist in the migration of .NET Core and .NET Framework projects to the latest .NET version. It offers a step-by-step process, covering everything from updating project files to API replacements.

#### Installation

You can install the Upgrade Assistant as a global tool from the command line:

```bash
dotnet tool install -g upgrade-assistant
```

#### Usage

Run the assistant on your solution or project:

```bash
upgrade-assistant upgrade <Path to Solution/Project File>
```

The Upgrade Assistant will walk you through the migration steps, offering suggestions and even implementing certain changes automatically.

## Pre-migration Steps

1. **Backup Your Code**: Before making any changes, create a backup of your existing codebase.
2. **Analyze Dependencies**: Check third-party libraries and NuGet packages for compatibility with .NET 8.
3. **Update To Latest Patch**: Make sure you're on the latest patch version of your current .NET Core SDK.

## Migration Steps

1. **Run the Upgrade Assistant**: As described above, this will guide you through the necessary changes.
2. **Manual Code Changes**: The Upgrade Assistant will catch most issues, but you may need to manually update some portions of your code.
3. **Update CI/CD Pipelines**: If you have automated build and deployment pipelines, make sure they're updated to use the .NET 8 SDK.
4. **Compile and Test**: Once changes are done, compile your project and run comprehensive tests to ensure everything works as expected.

## Post-migration Modernizations

1. **Performance Tuning**: Leverage new performance improvements in .NET 8.
2. **Adopt New Features**: Take advantage of new language features and APIs.
3. **Dependency Updates**: Check if there are newer versions of third-party packages specifically targeted for .NET 8, and update them.

## Known Issues, Incompatibilities, and Gotchas

- **Json.NET**: System.Text.Json is the default for .NET 8; if you're using Newtonsoft.Json, you may need to adjust your serialization/deserialization code.
- **EF Core**: If you're using Entity Framework, you'll need to update to EF Core compatible with .NET 8.
- **ASP.NET Core**: Some middleware components have been deprecated or replaced.
- **Docker Images**: If you’re using Docker, make sure to update the base images to .NET 8.
- **Nullable Reference Types**: These are enabled by default in .NET 8, which might require changes in your code to remove nullability warnings.
  
## Additional Resources

- [.NET 8 Breaking Changes](https://docs.microsoft.com/en-us/dotnet/core/compatibility/)
- [.NET API Analyzer](https://www.nuget.org/packages/Microsoft.DotNet.Analyzers.Compatibility/)

This document is intended as a standard reference for migrating from .NET Core to .NET 8. It includes key decision-making criteria, outlines the migration process, highlights potential pitfalls, and offers guidance for modernization post-migration.
