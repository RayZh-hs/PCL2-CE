# Codebase Overview

## Project Structure

- **PCL.Core/**: Core logic library (C#).
    - **App/**: Application services, configuration, lifecycle management.
    - **IO/**: File handling, downloading, compression.
    - **Link/**: Networking, multiplayer features.
    - **Logging/**: Logging infrastructure.
    - **Minecraft/**: Minecraft game logic, version management, launching.
    - **Net/**: Network utilities.
    - **UI/**: UI controls and helpers (backend).
- **Plain Craft Launcher 2/**: Main UI project (VB.NET/WPF).
    - **Pages/**: UI Pages (XAML/VB).
        - **PageSetup/**: Settings pages.
    - **Controls/**: Custom UI controls.
    - **Images/**: Image resources.
    - **Application.xaml**: App entry point.
    - **FormMain.xaml**: Main window.
- **PCL.Test/**: Unit tests.

## Key Components

- **Config**: Managed by `PCL.Core.App.Config` using attributes like `[ConfigItem]`.
- **Lifecycle**: Managed by `PCL.Core.App.Lifecycle`.
- **UI Navigation**: Handled in `FormMain.xaml` and `PageSetupLeft.xaml`.

## Technologies

- **Languages**: C# (Core), VB.NET (UI).
- **Framework**: .NET (WPF).
- **Build System**: MSBuild / SLNX.
