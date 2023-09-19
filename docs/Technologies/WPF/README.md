# WPF Upgrade Paths

## Overview

[WPF (Windows Presentation Foundation)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-7.0) continues to be a key player in the .NET ecosystem for building Windows desktop applications. The future of WPF is promising, thanks to its [Roadmap](https://github.com/dotnet/wpf/blob/main/roadmap.md), with enhancements and new features planned for [.NET 7](https://devblogs.microsoft.com/dotnet/wpf-on-dotnet-7/) and [.NET 8](https://devblogs.microsoft.com/dotnet/wpf-file-dialog-improvements-in-dotnet-8/).

## Upgrade Targets

Choosing the right migration path depends on several factors such as existing codebase, team expertise, and business requirements. Use the decision tree below to guide your migration path.

- Is the application currently WPF?
  - Yes
    - Do you want to maintain native desktop capabilities?
      - Yes: Continue with .NET WPF
      - No: Consider Blazor or .NET MAUI
  - No
    - Is cross-platform support needed?
      - Yes: .NET MAUI
      - No: .NET WPF or Windows App SDK

### .NET WPF

#### Why WPF?

For applications that already use WPF, upgrading to the latest .NET version provides the most straightforward migration path. You maintain existing functionalities while gaining performance improvements and new features.

- [Migrate to latest dotnet](https://learn.microsoft.com/en-us/dotnet/architecture/modernize-desktop/example-migration)

#### Unsupported Changes

Pay close attention to unsupported APIs and deprecated functionalities when moving to the latest .NET version.

#### Case Studies

- [eShopModernizing WinForms](https://github.com/dotnet-architecture/eShopModernizing/tree/main/eShopLegacyNTier/src/eShopWinForms): Built in net framework 4.7 on WinForms.
- [eShopModernizing WPF](https://github.com/dotnet-architecture/eShopModernizing/tree/main/eShopModernizedNTier/src/eShopWinForms): Migrated to .NET 6 and WPF

#### Pre-migration, migration, gotchas, and post-migration modernizations

1. **Pre-migration**
    - Backup codebase
    - Run API analysis for unsupported or deprecated APIs
2. **Migration**
    - Update project files and references
    - Refactor code for unsupported APIs
3. **Gotchas**
    - Possible issues with XAML layouts
    - Dependency injection changes
4. **Post-migration**
    - Leverage new features such as `INotifyDataErrorInfo`
    - Optimize performance

### Blazor

#### Why Blazor?

Blazor offers a pathway to leverage web technologies, which can be especially beneficial for teams with expertise in web development or for applications that are targeting both web and desktop.

- [Embed WPF in Blazor App](https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/wpf?view=aspnetcore-7.0)

#### Unsupported Changes

Some features like complex animations may not be supported in Blazor.

#### Case Studies

#### Pre-migration, migration, gotchas, and post-migration modernizations

1. **Pre-migration**
    - Evaluate if the app can be ported or if it needs a rewrite
    - Assess UI elements that can be reused
2. **Migration**
    - Port logic to Razor components
    - Integrate WPF elements as necessary
3. **Gotchas**
    - Client-side limitations for Blazor WebAssembly
    - Integration issues with WPF components
4. **Post-migration**
    - Utilize Blazor-specific features like SignalR
    - Progressive Web App (PWA) capabilities

### Windows App SDK

#### Why Windows App SDK?

Windows App SDK allows you to take advantage of the latest Windows 10 features while maintaining compatibility with older versions. It's a good option for modernizing WPF applications without a complete rewrite.

- [Modernize a WPF app](https://learn.microsoft.com/en-us/windows/apps/desktop/modernize/modernize-wpf-tutorial)
- [Embed Windows App SDK in WPF app](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/wpf-plus-winappsdk)

#### Unsupported Changes

Possible issues with migrating third-party libraries or components that are not compatible with Windows App SDK.

#### Case Studies

#### Pre-migration, migration, gotchas, and post-migration modernizations

1. **Pre-migration**
    - Assess which Windows 10 features are needed
    - Check for unsupported libraries
2. **Migration**
    - Update project files
    - Implement new APIs as necessary
3. **Gotchas**
    - Learning curve for new Windows 10 APIs
    - Possible breaking changes in existing functionalities
4. **Post-migration**
    - Implement Fluent UI
    - Utilize advanced packaging features

### .NET MAUI

#### Why MAUI?

.NET MAUI (Multi-platform App UI) is ideal if you need to extend your application to other platforms like Android and iOS while sharing most of your codebase.

- [Using the upgrade assistant](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/upgrade-assistant)

#### Unsupported Changes

Certain WPF features like 3D graphics are not supported in .NET MAUI.

#### Case Studies

#### Pre-migration, migration, gotchas, and post-migration modernizations

1. **Pre-migration**
    - Evaluate UI components for cross-platform compatibility
    - Check third-party library support
2. **Migration**
    - Rewrite UI in MAUI controls
    - Port logic code
3. **Gotchas**
    - MAUI's limitations compared to WPF for complex UI
    - Dependency changes
4. **Post-migration**
    - Adopt MVU (Model-View-Update) pattern for easier state management
    - Optimize performance across platforms

## Additional Resources

- [Community Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/introduction)

This document serves as a standard reference for migrating WPF applications. It outlines key considerations, provides real-world case studies, and highlights potential 'gotchas' to help guide your migration strategy.
