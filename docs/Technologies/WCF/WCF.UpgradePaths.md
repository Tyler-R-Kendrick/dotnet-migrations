# Migrating Away from WCF Services

## Overview

As WCF has been officially deprecated for .NET Core and newer versions, it's high time for enterprise customers to consider migrating to modern alternatives. This document will focus on commonly recommended migration paths such as gRPC, CoreWCF, RESTful Web APIs using ASP.NET Core, and Azure Service Fabric.

## Decision Matrix for Migration Targets

Here is a decision matrix to help you determine which migration target may best fit your needs:

| Feature / Requirement   | gRPC  | CoreWCF  | ASP.NET Core Web APIs  | Azure Service Fabric  |
|-------------------------|-------|----------|------------------------|-----------------------|
| HTTP/2 Support          | ✅    | ❌       | ✅                     | ❌                    |
| Streaming               | ✅    | ✅       | ✅                     | ✅                    |
| Request/Reply Pattern   | ✅    | ✅       | ✅                     | ✅                    |
| Publish/Subscribe       | ❌    | ✅       | ✅ (with SignalR)       | ✅                    |
| Language Agnostic       | ✅    | ❌       | ✅                     | ❌                    |
| Message Queuing         | ❌    | ✅       | ❌                     | ✅                    |
| Strongly Typed Contracts| ✅    | ✅       | ❌                     | ❌                    |
| Cross-platform          | ✅    | ❌       | ✅                     | ✅                    |

## gRPC

### Why Choose gRPC?

- Efficient HTTP/2-based communication
- Strongly-typed code-first approach
- Built-in support for multiple programming languages

### Steps for Migration

1. **Analyze Existing WCF Services**: List out all the services, operations, data contracts, and fault contracts.
2. **Install Grpc.Tools**: These tools provide the necessary runtime and compiler support for gRPC.
3. **Code-First Design**: Create Protobuf files based on your WCF contracts.
4. **Implement Services**: Implement service methods as defined in Protobuf.
5. **Test and Optimize**: Make sure to thoroughly test the new services and adjust them for performance.

### Known Issues

- No support for SOAP-based message format.
- Limited to HTTP/2.
  
### Case Studies

- [From WCF to gRPC: An End-to-End Example](https://www.infoq.com/articles/grpc-from-wcf/)

## CoreWCF

### Why Choose CoreWCF?

- Most similar to existing WCF framework
- Easier migration path if heavily invested in WCF

### Steps for Migration

1. **Update Contracts**: Make sure all your WCF contracts are in a separate contract library.
2. **Install CoreWCF**: Add the CoreWCF NuGet packages to your project.
3. **Update Configuration**: Translate WCF configuration to CoreWCF configuration.

### Known Issues

- Not as performance-optimized as gRPC
- Limited community support and features

### Case Studies

- [Migrating WCF Services to CoreWCF](https://corewcf.github.io/CoreWCF/)

## ASP.NET Core Web APIs

### Why Choose ASP.NET Core Web APIs?

- Good fit for RESTful services
- Large ecosystem and community support

### Steps for Migration

1. **Identify Operations**: Find out all service operations and their HTTP verb associations.
2. **Create Controllers**: Use ASP.NET Core to create corresponding controllers and actions.
3. **Update Clients**: Make sure to update client applications to use HTTP-based calls.

### Known Issues

- Requires rewriting service contracts as controller actions.

### Case Studies

- [Migrating WCF to ASP.NET Core Web API](https://docs.microsoft.com/en-us/dotnet/architecture/grpc-for-wcf-developers/migrate-wcf-to-web-api)

## Azure Service Fabric

### Why Choose Azure Service Fabric?

- Scalable and reliable
- Supports complex scenarios like stateful services and actor model

### Steps for Migration

1. **Identify Service Types**: Whether stateless or stateful.
2. **Create Service Fabric App**: Start a new Service Fabric application.
3. **Implement Services**: Transfer WCF service logic to Service Fabric services.

### Known Issues

- Complexity due to various service paradigms (stateless, stateful, actors)
  
### Case Studies

- [Migration of WCF services to Azure Service Fabric](https://azure.microsoft.com/en-us/resources/samples/service-fabric-dotnet-getting-started/)

## Additional Resources

- [gRPC Official Documentation](https://grpc.io/docs/)
- [CoreWCF GitHub Repo](https://github.com/CoreWCF/CoreWCF)
- [ASP.NET Core Web API Documentation](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-5.0)
- [Azure Service Fabric Documentation](https://docs.microsoft.com/en-us/azure/service-fabric/)

This guide aims to provide a clear migration path from WCF services to modern alternatives suitable for enterprise applications. Each section offers a justification for choosing a specific technology, steps to migrate, and known issues you might encounter during the migration process.
