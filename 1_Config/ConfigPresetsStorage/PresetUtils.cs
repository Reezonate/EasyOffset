using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace EasyOffset {
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
            var presetVersion = jObject.GetValue("version", StringComparison.OrdinalIgnoreCase)?.Value<string>();
            return presetVersion switch {
                ConfigPresetV2.Version => ConfigPresetV2.Deserialize(jObject),
                ConfigPresetV1.Version => ConfigPresetV1.Deserialize(jObject),
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

        #region GetPresetCellString
        
        public static string GetPresetCellString(StoredConfigPreset preset) {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<align=\"left\">");

            if (preset.LoadFailed) {
                stringBuilder.Append("<color=red>");
                stringBuilder.Append("load error");
            } else {
                stringBuilder.Append(GetTimeString(preset.ConfigPreset.UnixTimestamp));
            }

            stringBuilder.Append("<pos=30%>");
            stringBuilder.Append(preset.Name);

            return stringBuilder.ToString();
        }

        #endregion

        #region Utils

        private const int SecondsInMinute = 60;
        private const int SecondsInHour = 60 * SecondsInMinute;
        private const int SecondsInDay = 24 * SecondsInHour;

        private const int VeryOldTimeDifference = 365 * SecondsInDay;
        private const string FutureString = "o_O";
        private const string VeryOldString = "-";

        private static string GetTimeString(long unixTimestamp) {
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            var timeDifference = now - unixTimestamp;

            switch (timeDifference) {
                case < 0: return FutureString;
                case > VeryOldTimeDifference: return VeryOldString;
            }

            var fullDays = timeDifference / SecondsInDay;

            if (fullDays > 0) {
                return $"{fullDays} d ago";
            }

            var fullHours = timeDifference / SecondsInHour;
            if (fullHours > 0) {
                return $"{fullHours} h ago";
            }

            var fullMinutes = timeDifference / SecondsInMinute;
            return fullMinutes > 0 ? $"{fullMinutes} m ago" : $"{timeDifference} s ago";
        }

        #endregion
    }
}