import json
import os
import argparse

def load_lang_file(path) -> dict:
    with open(path, 'r', encoding='utf-8') as f:
        return json.load(f)

def compare_lang_files(base_path: str, compare_path: str):
    base_lang = load_lang_file(base_path)
    compare_lang = load_lang_file(compare_path)
    base_name = os.path.basename(base_path)
    compare_name = os.path.basename(compare_path)
    
    missing_in_compare = []
    for key in base_lang.keys():
        if key not in compare_lang:
            missing_in_compare.append(key)
    if missing_in_compare:
        print(f"Keys present in {base_name} but missing in {compare_name}:")
        for key in missing_in_compare:
            print(f"  - {key}")
    else:
        print(f"All keys in {base_name} are present in {compare_name}.")
    
    missing_in_base = []
    for key in compare_lang.keys():
        if key not in base_lang:
            missing_in_base.append(key)
    if missing_in_base:
        print(f"Keys present in {compare_name} but missing in {base_name}:")
        for key in missing_in_base:
            print(f"  - {key}")
    else:
        print(f"All keys in {compare_name} are present in {base_name}.")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Compare two language JSON files for missing keys.")
    parser.add_argument("base_file", help="Path to the base language file (e.g., en-US.json)")
    parser.add_argument("compare_file", help="Path to the language file to compare (e.g., zh-CN.json)")
    args = parser.parse_args()
    
    compare_lang_files(args.base_file, args.compare_file)