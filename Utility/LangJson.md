# LangJson Utility

A Python utility for managing, validating, and synchronizing multiple localization JSON files simultaneously. It resides in the `Utility` folder of the project.

## Usage

```bash
python LangJson.py <Command> [Arguments] [-Dir <path>]
```

**Global Arguments:**
- `-Dir`: (Optional) Path to the folder containing `.json` files. Drop it to use default json file position (recommended approach).
---

## Commands

### 1. Clean
Sorts keys alphabetically in **all** JSON files.
```bash
python LangJson.py Clean
```

### 2. Check
Validates that all JSON files contain the exact same set of keys.
- **Exit Code 0**: Success (all keys match).
- **Exit Code 1**: Failure (missing keys found).
```bash
python LangJson.py Check
```

### 3. Get
Retrieves the value of a specific key from all files.
```bash
python LangJson.py Get "Title.Launch"
```

### 4. Remove
Removes a specific key from all files.
```bash
python LangJson.py Remove "Title.OldFeature"
```

### 5. Add
Adds a new key to all files. Keys are auto-sorted after addition.

**Interactive Mode:**
Prompts for input for every language found in the directory.
```bash
python LangJson.py Add "Title.NewFeature"
```

**Non-Interactive Mode:**
Requires the `-NonInteractive` flag. You must provide a flag for language files to add to (e.g., `-en-US` for `en-US.json`).
```bash
python LangJson.py Add "Title.NewFeature" -NonInteractive -en-US="New Feature" -zh-CN="新功能"
```

### 6. New
Creates a new language file by duplicating `en-US.json`. The filename is derived from the language key.
```bash
python LangJson.py New fr-FR
# Creates fr-FR.json
```