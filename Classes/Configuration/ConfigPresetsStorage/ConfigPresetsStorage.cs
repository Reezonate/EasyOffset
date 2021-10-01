using System.Collections.Generic;
using System.IO;
using IPA.Utilities;

namespace EasyOffset.Configuration {
    public static class ConfigPresetsStorage {
        private static readonly string PresetsFolderPath = Path.Combine(UnityGame.UserDataPath, "EasyOffset\\Presets\\");

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