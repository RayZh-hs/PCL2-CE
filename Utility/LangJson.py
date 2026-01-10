import json
import os
import argparse
import sys
import glob

# Configuration
DEFAULT_INDENT = 4

def get_json_files(directory):
    """Finds all .json files in the specified directory."""
    search_path = os.path.join(directory, "*.json")
    files = glob.glob(search_path)
    if not files:
        print(f"No .json files found in {directory}")
        sys.exit(1)
    return sorted(files)

def load_json(path):
    with open(path, 'r', encoding='utf-8') as f:
        return json.load(f)

def save_json(path, data):
    with open(path, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=DEFAULT_INDENT)

def cmd_clean(args, extras):
    """Sort keys alphabetically in all files."""
    files = get_json_files(args.Dir)
    print(f"Cleaning (sorting keys) for {len(files)} files...")
    
    for file_path in files:
        data = load_json(file_path)
        sorted_data = dict(sorted(data.items()))
        save_json(file_path, sorted_data)
        print(f"  - Sorted {os.path.basename(file_path)}")
    print("Done.")

def cmd_check(args, extras):
    """Check for missing keys across all files against the union of all keys."""
    files = get_json_files(args.Dir)
    loaded_files = {}
    all_keys = set()

    for file_path in files:
        data = load_json(file_path)
        loaded_files[file_path] = data
        all_keys.update(data.keys())
    
    has_error = False
    
    for file_path, data in loaded_files.items():
        file_name = os.path.basename(file_path)
        missing = sorted(list(all_keys - data.keys()))
        
        if missing:
            has_error = True
            print(f"File '{file_name}' is missing {len(missing)} keys:")
            for key in missing:
                print(f"  - {key}")
            print("-" * 20)
    
    if has_error:
        print("Check failed: Missing keys detected.")
        sys.exit(1)
    else:
        print("Check passed: All files contain matching keys.")
        sys.exit(0)

def cmd_get(args, extras):
    """Get value of a specific key from all files."""
    files = get_json_files(args.Dir)
    key = args.key
    found_any = False

    print(f"Values for key '[{key}]':")
    for file_path in files:
        data = load_json(file_path)
        file_name = os.path.basename(file_path)
        if key in data:
            found_any = True
            print(f"  {file_name:<20}: {data[key]}")
        else:
            print(f"  {file_name:<20}: <MISSING>")
    
    if not found_any:
        print("Key not found in any file.")

def cmd_remove(args, extras):
    """Remove a key from all files."""
    files = get_json_files(args.Dir)
    key = args.key
    
    print(f"Removing key '[{key}]'...")
    for file_path in files:
        data = load_json(file_path)
        if key in data:
            del data[key]
            save_json(file_path, data)
            print(f"  - Removed from {os.path.basename(file_path)}")
        else:
            print(f"  - Not present in {os.path.basename(file_path)}")
    print("Done.")

def cmd_add(args, extras):
    """Add a new key to all files (Interactive or Non-Interactive)."""
    files = get_json_files(args.Dir)
    key = args.key
    loaded_data = {}

    # Load all files first
    for file_path in files:
        loaded_data[file_path] = load_json(file_path)

    # --- NON-INTERACTIVE MODE ---
    if args.non_interactive:
        # Parse extras (e.g. ['-en-US=Val', '-zh-CN=Val'])
        provided_translations = {}
        for item in extras:
            if item.startswith("-"):
                # Remove leading dashes
                clean_item = item.lstrip("-") 
                if "=" in clean_item:
                    lang, val = clean_item.split("=", 1)
                    provided_translations[lang] = val
        
        # # Validate that we have a translation for EVERY file
        # missing_langs = []
        # for file_path in files:
        #     file_name = os.path.basename(file_path)
        #     lang_code = os.path.splitext(file_name)[0]
        #     if lang_code not in provided_translations:
        #         missing_langs.append(lang_code)
        
        # if missing_langs:
        #     print(f"Error: Non-Interactive mode requires values for ALL languages.")
        #     print(f"Missing flags for: {', '.join(missing_langs)}")
        #     print(f"Expected format: -NonInteractive -{missing_langs[0]}=\"Value\" ...")
        #     sys.exit(1)

        # Apply changes
        print(f"Adding key '[{key}]' to {len(files)} files (Non-Interactive)...")
        for file_path in files:
            file_name = os.path.basename(file_path)
            lang_code = os.path.splitext(file_name)[0]
            val = provided_translations[lang_code]
            loaded_data[file_path][key] = val

    # --- INTERACTIVE MODE ---
    else:
        # Sort files to prioritize en-US.json
        files.sort(key=lambda x: (0 if 'en-US.json' in x else 1, x))
        print(f"Adding key '[{key}]'. Please enter values for each language:")
        
        try:
            for file_path in files:
                file_name = os.path.basename(file_path)
                lang_code = os.path.splitext(file_name)[0]
                
                existing_val = loaded_data[file_path].get(key, "")
                prompt_text = f"({lang_code}) "
                if existing_val:
                    print(f"  Current value: {existing_val}")
                    prompt_text = f"({lang_code}) [Enter to keep]: "

                val = input(prompt_text)
                
                if existing_val and val == "":
                    pass 
                else:
                    loaded_data[file_path][key] = val
        except KeyboardInterrupt:
            print("\nOperation cancelled.")
            sys.exit(1)

    # Save all
    for file_path, data in loaded_data.items():
        sorted_data = dict(sorted(data.items()))
        save_json(file_path, sorted_data)
    
    print("Key added and files saved.")

def cmd_new(args, extras):
    """Create a new language file by duplicating en-US.json."""
    lang_key = args.language_key
    if not lang_key.lower().endswith('.json'):
        new_filename = f"{lang_key}.json"
    else:
        new_filename = lang_key
        
    new_path = os.path.join(args.Dir, new_filename)
    
    if os.path.exists(new_path):
        print(f"Error: File {new_filename} already exists.")
        sys.exit(1)
        
    source_filename = "en-US.json"
    source_path = os.path.join(args.Dir, source_filename)
    
    if not os.path.exists(source_path):
        files = get_json_files(args.Dir)
        source_path = files[0]
        print(f"Warning: en-US.json not found. Copying from {os.path.basename(source_path)}.")
    
    data = load_json(source_path)
    if "Language" in data:
        data["Language"] = os.path.splitext(new_filename)[0]
        
    save_json(new_path, data)
    print(f"Created new language file: {new_path}")

def main():
    parser = argparse.ArgumentParser(description="Manage Language JSON files.")
    
    # Get script directory
    script_dir = os.path.dirname(os.path.abspath(__file__))
    default_lang_dir = os.path.join(script_dir, "..", "Plain Craft Launcher 2", "Languages")
    parser.add_argument("-Dir", default=default_lang_dir, help="Directory containing JSON files")
    
    subparsers = parser.add_subparsers(dest="command", required=True)
    
    # Clean
    p_clean = subparsers.add_parser("Clean")
    p_clean.set_defaults(func=cmd_clean)
    
    # Check
    p_check = subparsers.add_parser("Check")
    p_check.set_defaults(func=cmd_check)
    
    # Get
    p_get = subparsers.add_parser("Get")
    p_get.add_argument("key")
    p_get.set_defaults(func=cmd_get)
    
    # Remove
    p_remove = subparsers.add_parser("Remove")
    p_remove.add_argument("key")
    p_remove.set_defaults(func=cmd_remove)
    
    # Add
    p_add = subparsers.add_parser("Add")
    p_add.add_argument("key")
    p_add.add_argument("-NonInteractive", dest="non_interactive", action="store_true", help="Enable non-interactive mode")
    p_add.set_defaults(func=cmd_add)
    
    # New
    p_new = subparsers.add_parser("New")
    p_new.add_argument("language_key")
    p_new.set_defaults(func=cmd_new)
    
    # Use parse_known_args to capture dynamic flags (like -zh-CN="...")
    args, unknown = parser.parse_known_args()
    
    if not os.path.isdir(args.Dir):
        print(f"Error: Directory '{args.Dir}' does not exist.")
        sys.exit(1)
        
    args.func(args, unknown)

if __name__ == "__main__":
    main()