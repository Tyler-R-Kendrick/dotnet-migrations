# ASP.NET Upgrade Paths

## ASP.NET Core

[link](https://learn.microsoft.com/en-us/aspnet/core/migration/inc/overview?view=aspnetcore-7.0)
[incremental upgrades](https://learn.microsoft.com/en-us/aspnet/core/migration/inc/overview?view=aspnetcore-7.0)

## Overview

ASP.NET Web Forms has been a staple for web development in the .NET ecosystem but lacks features present in modern frameworks. ASP.NET Core and Blazor offer more modular, scalable, and maintainable approaches. This document outlines the best practices for migrating from ASP.NET Web Forms to these modern technologies.

## Decision Tree for Migration

- Is your application heavily dependent on ViewState and PostBack?
  - Yes: Migration will require significant redesign; consider ASP.NET Core MVC.
  - No: You can move to either ASP.NET Core MVC, Razor Pages, or Blazor.

- Do you need real-time web features like WebSockets?
  - Yes: ASP.NET Core or Blazor with SignalR is an excellent option.
  - No: Both ASP.NET Core and Blazor are still valid options.

- Is client-side rendering important for your application?
  - Yes: Consider Blazor WebAssembly.
  - No: Blazor Server or ASP.NET Core are better suited.

### Before Migration: ASP.NET Web Forms Example

You might have an ASP.NET Web Forms page with code-behind logic:

```aspx
<!-- Default.aspx -->
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsApp._Default" %>
<asp:Button ID="SubmitButton" runat="server" Text="Submit" OnClick="SubmitButton_Click" />
```

```csharp
// Default.aspx.cs
protected void SubmitButton_Click(object sender, EventArgs e)
{
    // Logic here
}
```

### After Migration: ASP.NET Core Example

In ASP.NET Core MVC, this could be transformed into:

```html
<!-- Views/Home/Index.cshtml -->
<button asp-controller="Home" asp-action="Submit">Submit</button>
```

```csharp
// Controllers/HomeController.cs
public IActionResult Submit()
{
    // Logic here
    return View();
}
```

### After Migration: Blazor Example

Or, in Blazor:

```html
<!-- Pages/Index.razor -->
<button @onclick="Submit">Submit</button>
```

```csharp
// Pages/Index.razor.cs
private void Submit()
{
    // Logic here
}
```

## Steps for Migrating

1. **Assess Current Application**: Identify dependencies, features, and overall architecture.
2. **Choose Migration Target**: Use the decision tree to choose between ASP.NET Core and Blazor.
3. **Set Up New Project**: Create a new ASP.NET Core or Blazor project in your solution.
4. **Port Business Logic**: Migrate code-behind logic, models, and utilities to the new project.
5. **Recreate UI**: Rewrite the UI using Razor syntax for ASP.NET Core or Blazor components.
6. **Implement Dependency Injection**: Replace legacy IoC containers or `HttpContext.Current` dependencies.
7. **Test**: Conduct exhaustive tests to verify the migration.

## Known Issues, Incompatibilities, and Gotchas

- **ViewState**: ViewState doesn't exist in modern platforms. Any logic dependent on it will need to be redesigned.
- **Page Lifecycle**: Page lifecycle events differ between Web Forms and ASP.NET Core/Blazor.
- **User Controls**: Web Forms User Controls (.ascx) do not have a direct equivalent and will need to be converted to partial views or components.
- **Authentication**: Forms Authentication or other legacy auth mechanisms must be migrated to ASP.NET Core Identity or JWT.
  
## Libraries to be Upgraded

- Entity Framework: Upgrade to EF Core.
- SignalR: Old versions should be updated to ASP.NET Core SignalR.
- Various NuGet packages: Ensure compatibility with ASP.NET Core or Blazor.

## Additional Resources

- [Migrating from ASP.NET to ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/migration/proper-to-2x/?view=aspnetcore-5.0)
- [Migrate from ASP.NET Web Forms to Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/migration/web-forms?view=aspnetcore-5.0)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-5.0)

This document serves as a comprehensive guideline for migrating from ASP.NET Web Forms to modern alternatives like ASP.NET Core and Blazor. It includes a decision tree for migration strategies, sample code snippets, and tips on handling common issues and challenges.
