import json
import os

BASE_PATH = r"Plain Craft Launcher 2/Languages"
CONFIG_FILE = "LangJsonAddKeysConfig.json"


def load_json(path):
    if not os.path.exists(path):
        return {}
    with open(path, "r", encoding="utf-8") as f:
        return json.load(f)


def save_json(path, data):
    with open(path, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=4, ensure_ascii=False)


def apply_config(config_path, base_lang_path):
    config = load_json(config_path)
    if not config:
        print("Config file is empty or missing.")
        return

    # Cache language files
    lang_cache = {}

    for key, translations in config.items():
        if not isinstance(translations, dict):
            print(f"Skipped {key} (invalid format)")
            continue

        for lang, value in translations.items():
            lang_file = os.path.join(base_lang_path, f"{lang}.json")

            if lang not in lang_cache:
                lang_cache[lang] = load_json(lang_file)

            lang_data = lang_cache[lang]

            if key in lang_data:
                print(f"[{lang}] Skipped {key} (exists)")
            else:
                lang_data[key] = value
                print(f"[{lang}] Added {key}")

    # Save all modified language files
    for lang, data in lang_cache.items():
        save_json(os.path.join(base_lang_path, f"{lang}.json"), data)


if __name__ == "__main__":
    apply_config(CONFIG_FILE, BASE_PATH)
