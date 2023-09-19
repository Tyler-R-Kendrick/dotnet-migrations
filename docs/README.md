# Dotnet Migration Paths

## Technology Upgrades

[Example](https://learn.microsoft.com/en-us/dotnet/architecture/modernize-desktop/example-migration)

## Readiness Assessment Standards

### Overview

This document provides a set of standards to help you analyze, assess, and prepare your .NET projects for migration or modernization. Following this guidance will facilitate smoother transitions, fewer post-migration issues, and a more robust modernized application.

### Steps for Readiness Assessment

#### 1. Inventory Analysis

**Objective**: Create an inventory of all application assets.

- List all projects in the solution.
- List all third-party dependencies, including NuGet packages.
- Identify custom libraries or frameworks used.
  
#### 2. Codebase Scanning

**Objective**: Analyze codebase for potential migration blockers and code smells.

- Run a static code analysis using tools like SonarQube or .NET analyzers.
- Identify deprecated APIs or libraries.
- Look for tightly-coupled components or poor separation of concerns.

#### 3. Architecture Review

**Objective**: Understand the architecture to identify potential issues with scalability, maintainability, and extensibility.

- Draw or update architecture diagrams.
- Identify components that are difficult to decouple (e.g., business logic mixed with UI).
  
#### 4. Data Assessment

**Objective**: Examine database schemas, data models, and data storage solutions.

- List all databases and data stores.
- Document any potential data migration issues, such as deprecated data types or unsupported data stores in the modern stack.

#### 5. Compliance and Security

**Objective**: Ensure that the application complies with regulatory and security standards.

- Document the security protocols currently in place.
- Note down any compliance issues that need to be addressed during migration.

#### 6. Business Logic Auditing

**Objective**: Distill the core business logic from existing codebase for more straightforward migration.

- Identify core business processes and rules.
- List all APIs that interact with these processes.

#### 7. Test Coverage Analysis

**Objective**: Evaluate the state of unit tests, integration tests, and UI tests.

- Run existing tests and document coverage percentages.
- Note down areas with insufficient test coverage for future attention.

#### 8. Migration Feasibility Report

**Objective**: Consolidate all findings into a report that can guide the migration process.

- Enumerate migration blockers.
- Suggest alternatives for deprecated or unsupported features.
- Provide an estimated effort for migration in terms of man-hours.

### Tools and Resources

#### Inventory and Codebase Analysis

- Visual Studio Solution Explorer
- [NuGet Package Manager](https://www.nuget.org/)
- [SonarQube](https://www.sonarqube.org/)
  
#### Architecture and Data Assessment

- [Microsoft Threat Modeling Tool](https://aka.ms/threatmodelingtool)
- [Entity Framework Core Power Tools](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EntityFrameworkCorePowerTools)

#### Testing

- [xUnit](https://xunit.net/)
- [Moq](https://github.com/moq/moq4)
- [Selenium](https://www.selenium.dev/)

#### Reporting

- Microsoft Word or Google Docs for documentation
- [draw.io](https://app.diagrams.net/) for architectural diagrams

### Summary

Performing a thorough readiness assessment before migrating or modernizing your application sets the stage for a successful transition. These standards cover the necessary dimensions for a comprehensive evaluation and provide actionable items to ensure your project's success.

## Optimizing for the future

### Using DI, Inversion of Control, and SOLID principles

### Abstractions Packages, the hosting layer, and Package Design Patterns

### Containerization, Bridge to Kubernetes, and DevX

### Sdks, Workloads, Generators, Analyzers, and Copilots

## Migration Patterns

### Incremental upgrades

### Strangler Pattern

## Modern Application Blocks

### Dependency Injection

### Generic Repositories and Data Access

### Validation

### Transient Fault Handling

### Configuration

### Logging, Monitoring, and Telemetry

### Auth and Identity

### Policy Injection

### Caching
