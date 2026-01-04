# Build and Run Instructions

This document provides instructions on how to build and run PCL2-CE with the internationalization (i18n) features.

## Prerequisites

1. **.NET 8 Desktop Runtime** (for running the application)
   - Download from: [https://get.dot.net/8](https://get.dot.net/8)
   
2. **.NET 8 SDK** (for building from source)
   - Download from: [https://get.dot.net/8](https://get.dot.net/8)

3. **Visual Studio 2022** or **Visual Studio Code** with .NET extensions

## Building from Source

### Using Visual Studio

1. Open the solution file `Plain Craft Launcher 2.slnx` in Visual Studio 2022
2. Select the desired configuration (Debug, Release, or Beta)
3. Select the desired platform (AnyCPU, x64, or ARM64)
4. Build the solution (Build → Build Solution or Ctrl+Shift+B)
5. The executable will be created in:
   - Debug: `Plain Craft Launcher 2\bin\Debug\`
   - Release: `Plain Craft Launcher 2\bin\Release\`
   - Beta: `Plain Craft Launcher 2\bin\Beta\`

### Using Command Line

1. Open a command prompt or PowerShell in the project root directory
2. Run one of the following commands based on your desired configuration:

```bash
# Debug build
dotnet build "Plain Craft Launcher 2/Plain Craft Launcher 2.vbproj" --configuration Debug

# Release build
dotnet build "Plain Craft Launcher 2/Plain Craft Launcher 2.vbproj" --configuration Release

# Beta build
dotnet build "Plain Craft Launcher 2/Plain Craft Launcher 2.vbproj" --configuration Beta
```

## Running the Application

### After Building

1. Navigate to the appropriate output directory:
   - Debug: `Plain Craft Launcher 2\bin\Debug\`
   - Release: `Plain Craft Launcher 2\bin\Release\`
   - Beta: `Plain Craft Launcher 2\bin\Beta\`

2. Run `Plain Craft Launcher 2.exe`

### Using Pre-built Releases

1. Download the latest release from: [https://github.com/PCL-Community/PCL2-CE/releases](https://github.com/PCL-Community/PCL2-CE/releases)
2. Extract the downloaded archive
3. Run `Plain Craft Launcher 2.exe`

## Testing Internationalization Features

1. **First Launch Experience**:
   - Delete or rename the configuration file to trigger first launch
   - The language selection dialog should appear
   - Select either Chinese (简体中文) or English

2. **Language Switching**:
   - Launch the application
   - Navigate to Settings → General
   - Change the language selection in the dropdown
   - A notification should appear indicating that a restart is required
   - Restart the application to see the language change

3. **Testing Fallback Behavior**:
   - In debug mode, the I18nServiceTest will run automatically
   - Check the debug output for test results
   - Tests verify:
     - Language file loading
     - Translation retrieval
     - Fallback behavior for missing keys
     - Behavior with missing language files

## Language Files

Language files are located in `Plain Craft Launcher 2/Languages/`:
- `zh-CN.json` - Chinese translations (default/fallback)
- `en-US.json` - English translations

To add a new language:
1. Create a new JSON file with the appropriate language code (e.g., `fr-FR.json`)
2. Add translations for all keys
3. Add the language to the ComboBox in `PageSetupSystem.xaml`
4. Update the language selection dialog if needed

## Troubleshooting

1. **Missing .NET Runtime**: Ensure .NET 8 Desktop Runtime is installed
2. **Language Not Changing**: Make sure to restart the application after changing language
3. **Missing Translations**: Check that the language files exist and contain the required keys
4. **Build Errors**: Ensure all NuGet packages are restored (Build → Restore NuGet Packages)

## Project Structure

- `PCL.Core/` - Core library with shared functionality
- `Plain Craft Launcher 2/` - Main WPF application
- `PCL.Test/` - Test projects
- `Docs/` - Documentation
- `Todo/` - Task tracking