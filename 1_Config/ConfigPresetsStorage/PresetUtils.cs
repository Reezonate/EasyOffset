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

        private const int TimeColumnCharCount = 12;
        private const int NameColumnCharCount = 25;

        public static string GetPresetCellString(StoredConfigPreset preset) {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<align=\"left\">");
            stringBuilder.Append("<mspace=0.4em>");

            if (preset.LoadFailed) {
                stringBuilder.Append("<color=red>");
                stringBuilder.AppendFixed("load error", TimeColumnCharCount);
            } else
                switch (preset.ConfigPreset.ConfigVersion) {
                    default:
                        stringBuilder.AppendTimeString(preset.ConfigPreset.UnixTimestamp, TimeColumnCharCount);
                        break;
                }

            stringBuilder.Append("</mspace>");
            stringBuilder.AppendFixed(preset.Name, NameColumnCharCount);

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

        private static void AppendTimeString(this StringBuilder stringBuilder, long unixTimestamp, int charCount) {
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            var timeDifference = now - unixTimestamp;

            switch (timeDifference) {
                case < 0:
                    stringBuilder.AppendFixed(FutureString, charCount);
                    break;
                case > VeryOldTimeDifference:
                    stringBuilder.AppendFixed(VeryOldString, charCount);
                    break;
                default:
                    var fullDays = timeDifference / SecondsInDay;
                    if (fullDays > 0) {
                        stringBuilder.AppendFixed($"{fullDays} d ago", charCount);
                    } else {
                        timeDifference -= fullDays * SecondsInDay;
                        var fullHours = timeDifference / SecondsInHour;
                        if (fullHours > 0) {
                            stringBuilder.AppendFixed($"{fullHours} h ago", charCount);
                        } else {
                            timeDifference -= fullHours * SecondsInHour;
                            var fullMinutes = timeDifference / SecondsInMinute;
                            if (fullMinutes > 0) {
                                stringBuilder.AppendFixed($"{fullMinutes} m ago", charCount);
                            } else {
                                timeDifference -= fullMinutes * SecondsInMinute;
                                stringBuilder.AppendFixed($"{timeDifference} s ago", charCount);
                            }
                        }
                    }

                    break;
            }
        }

        private static void AppendFixed(this StringBuilder stringBuilder, string str, int charCount) {
            var overflow = str.Length > charCount;

            for (var i = 0; i < charCount; i++) {
                if (i >= str.Length) {
                    stringBuilder.Append(' ');
                    continue;
                }

                if (overflow && i > (charCount - 4)) {
                    stringBuilder.Append('.');
                    continue;
                }

                stringBuilder.Append(str[i]);
            }
        }

        #endregion
    }
}