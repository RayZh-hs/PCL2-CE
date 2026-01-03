# I18n Implementation Summary

This document summarizes the internationalization (i18n) implementation for PCL2-CE, supporting English (en-US) and Chinese (zh-CN) languages.

## Implementation Details

### Core Infrastructure

1. **I18nService** (`PCL.Core/App/I18nService.cs`)
   - Core service for loading language files and providing translations
   - Supports fallback to zh-CN when a key is missing in the selected language
   - Provides event notification for language changes
   - Handles missing or corrupt language files gracefully

2. **Language Files**
   - Located in `Plain Craft Launcher 2/Languages/` directory
   - JSON format with key-value pairs
   - `zh-CN.json` - Chinese translations (default/fallback language)
   - `en-US.json` - English translations

3. **Configuration**
   - `Config.Language` property stores the current language code
   - Language selection persists across application restarts

### UI Components

1. **Language Selection Dialog** (`PageSelectLanguage.xaml/.xaml.vb`)
   - Shown on first launch when no language is configured
   - Allows users to select between Chinese and English
   - Uses translation markup extensions for UI text

2. **Settings Integration** (`PageSetupSystem.xaml/.xaml.vb`)
   - Language ComboBox in Settings → General
   - Shows "Restart to Apply" notification when language changes
   - Selection is bound to `Config.Language`

3. **XAML Markup Extension** (`PCL.Core/UI/TranslateExtension.cs`)
   - Allows direct translation in XAML using `{i18n:Translate Key='Key'}`
   - Automatically updates UI when language changes
   - Supports dynamic translation without requiring application restart

### Application Flow

1. **First Launch**
   - Application checks if `Config.Language` is empty
   - Shows language selection dialog if no language is configured
   - Main window only loads after language selection

2. **Language Change**
   - User selects new language in settings
   - Service loads new language file
   - UI elements with markup extensions update automatically
   - User is notified that restart is required for full application language change

3. **Fallback Behavior**
   - If a key is missing in the selected language, falls back to zh-CN
   - If a language file is missing or corrupt, falls back to zh-CN
   - If a key is missing in both languages, returns the key itself

### Testing

1. **I18nServiceTest** (`PCL.Core/App/I18nServiceTest.cs`)
   - Verifies language files exist and can be loaded
   - Tests translation retrieval
   - Tests fallback behavior for missing keys
   - Tests behavior with missing language files
   - Runs in debug mode to verify implementation

## Usage Examples

### In Code
```csharp
// Get a translated string
string text = I18nService.Get("Language");
```

### In XAML
```xml
<!-- Using the markup extension -->
<TextBlock Text="{i18n:Translate Key='LaunchButton'}" />
```

## Supported Languages

- **zh-CN** - Simplified Chinese (default/fallback)
- **en-US** - English

## Adding New Languages

1. Create a new JSON file in `Languages/` directory (e.g., `fr-FR.json`)
2. Add translations for all keys in the new language file
3. Add the new language to the ComboBox in `PageSetupSystem.xaml`
4. Update the language selection dialog if needed

## Future Improvements

1. Support for more languages
2. Automatic language detection based on system locale
3. Language-specific resource files (images, etc.)
4. Pluralization support
5. Right-to-left language support