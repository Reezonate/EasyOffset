using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace EasyOffset.Configuration {
    internal static class PresetUtils {
        #region Read

        public static bool ReadPresetFromFile(string filePath, out IConfigPreset configPreset) {
            try {
                var rawFileString = File.ReadAllText(filePath);
                var jObject = JObject.Parse(rawFileString);
                configPreset = ParsePresetFromJson(jObject);
                return true;
            } catch (Exception) {
                configPreset = null;
                return false;
            }
        }

        private static IConfigPreset ParsePresetFromJson(JObject jObject) {
            var presetVersion = jObject["version"]?.Value<string>();
            return presetVersion switch {
                "1.0" => ConfigPresetV1.Deserialize(jObject),
                null => throw new Exception("Preset version is not specified"),
                _ => throw new Exception($"Unknown preset version: {presetVersion}")
            };
        }

        #endregion

        #region Write

        public static bool WritePresetToFile(string filePath, IConfigPreset configPreset) {
            try {
                var jObject = configPreset.Serialize();
                File.WriteAllText(filePath, jObject.ToString(), Encoding.UTF8);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        #endregion
    }
}