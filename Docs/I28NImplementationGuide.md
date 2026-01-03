# Internationalization (i18n) Implementation Guide

## Overview

The project uses a JSON-based system for multi-language support. Language files are stored in the `Languages/` directory.

## Adding a New Language

1.  Create a new JSON file in `Plain Craft Launcher 2/Languages/` (e.g., `fr-FR.json`).
2.  The file name should match the language code (e.g., `zh-CN`, `en-US`).
3.  Add key-value pairs for translations:
    ```json
    {
        "Key": "Translated Text"
    }
    ```
4.  Add the new language to the `ComboLanguage` in `Plain Craft Launcher 2/Pages/PageSetup/PageSetupSystem.xaml`.

## Using Translations in Code

Use the `PCL.Core.App.I18nService` to retrieve translated strings:

```csharp
string text = I18nService.Get("Key");
```

## Using Translations in XAML

Currently, you need to bind to a property that returns the translated string, or use a MarkupExtension (recommended for future improvement).

## Current Implementation Details

- **Config**: `Config.Language` stores the current language code.
- **Service**: `PCL.Core.App.I18nService` loads the JSON file and provides translations.
- **UI**: Language selection is available in "Settings -> General" (`PageSetupSystem.xaml`).
- **First Launch**: Logic in `Application.xaml.vb` checks if `Config.Language` is set.

## Future Improvements

- Implement a XAML MarkupExtension (e.g., `{i18n:Translate Key}`) for easier binding.
- Create a dedicated "First Launch" wizard to ask for language.
- Restart the application automatically when the language changes.
