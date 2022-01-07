using System;
using System.Collections.Generic;

namespace EasyOffset {
    public static class WarningLevelUtils {
        private static readonly Dictionary<string, WarningLevel> NameToTypeDictionary = new();
        public static readonly object[] AllNamesObjects;

        #region Constructor

        static WarningLevelUtils() {
            var allEnumValues = typeof(WarningLevel).GetEnumValues();

            AllNamesObjects = new object[allEnumValues.Length];
            var index = 0;

            foreach (WarningLevel type in allEnumValues) {
                var name = TypeToName(type);
                NameToTypeDictionary.Add(name, type);
                AllNamesObjects[index++] = name;
            }
        }

        #endregion

        #region NameToType

        public static WarningLevel NameToTypeOrDefault(string name) {
            return NameToTypeDictionary.ContainsKey(name) ? NameToTypeDictionary[name] : WarningLevel.Critical;
        }

        public static WarningLevel NameToType(string name) {
            return NameToTypeDictionary[name];
        }

        #endregion

        #region TypeToName

        public static string TypeToName(WarningLevel type) {
            return type switch {
                WarningLevel.NonCritical => "Everything",
                WarningLevel.Critical => "Critical only",
                WarningLevel.Disable => "Nothing",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}