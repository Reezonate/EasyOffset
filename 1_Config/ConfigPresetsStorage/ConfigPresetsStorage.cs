using System.Collections.Generic;
using System.IO;
using IPA.Utilities;

namespace EasyOffset {
    public static class ConfigPresetsStorage {
        private static readonly string PresetsFolderPath = Path.Combine(UnityGame.UserDataPath, "EasyOffset\\Presets\\");

        #region SaveCurrentPreset

        public static bool SaveCurrentPreset(string fileName) {
            var absolutePath = Path.Combine(PresetsFolderPath, $"{fileName}.json");
            return PresetUtils.WritePresetToFile(absolutePath, PluginConfig.GeneratePreset());
        }

        #endregion

        #region LoadPreset

        public static bool LoadPreset(string fileName) {
            var absolutePath = Path.Combine(PresetsFolderPath, $"{fileName}.json");
            if (!PresetUtils.ReadPresetFromFile(absolutePath, out var preset)) return false;
            PluginConfig.ApplyPreset(preset);
            return true;
        }

        #endregion

        #region GetAllStoredPresets

        public static List<StoredConfigPreset> GetAllStoredPresets() {
            CreateDirectoryIfNecessary();

            var allPresets = new List<StoredConfigPreset>();

            foreach (var absoluteFilePath in GetAllJsonFilePaths()) {
                var successful = PresetUtils.ReadPresetFromFile(absoluteFilePath, out var preset);
                var storedConfigPreset = new StoredConfigPreset(
                    absoluteFilePath,
                    !successful,
                    preset
                );
                allPresets.Add(storedConfigPreset);
            }

            allPresets.Sort();
            allPresets.Reverse();
            return allPresets;
        }

        #endregion

        #region Utils

        private static string[] GetAllJsonFilePaths() {
            return Directory.GetFiles(PresetsFolderPath, "*.json");
        }

        private static void CreateDirectoryIfNecessary() {
            var isDirectoryPresent = Directory.Exists(PresetsFolderPath);
            if (!isDirectoryPresent) Directory.CreateDirectory(PresetsFolderPath);
        }

        #endregion
    }
}