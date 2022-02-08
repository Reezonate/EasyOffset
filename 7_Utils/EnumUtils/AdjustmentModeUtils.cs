using System;
using System.Collections.Generic;

namespace EasyOffset {
    public static class AdjustmentModeUtils {
        private static readonly Dictionary<string, AdjustmentMode> NameToTypeDictionary = new();
        public static readonly object[] AllNamesObjects;

        #region Constructor

        static AdjustmentModeUtils() {
            var allEnumValues = typeof(AdjustmentMode).GetEnumValues();

            AllNamesObjects = new object[allEnumValues.Length];
            var index = 0;

            foreach (AdjustmentMode type in allEnumValues) {
                var name = TypeToName(type);
                NameToTypeDictionary.Add(name, type);
                AllNamesObjects[index++] = name;
            }
        }

        #endregion

        #region NameToType

        public static AdjustmentMode NameToTypeOrDefault(string name) {
            return NameToTypeDictionary.ContainsKey(name) ? NameToTypeDictionary[name] : AdjustmentMode.None;
        }

        public static AdjustmentMode NameToType(string name) {
            return NameToTypeDictionary[name];
        }

        #endregion

        #region TypeToName

        public static string TypeToName(AdjustmentMode type) {
            return type switch {
                AdjustmentMode.None => "None",
                AdjustmentMode.Basic => "Basic",
                AdjustmentMode.Position => "Position",
                AdjustmentMode.Rotation => "Rotation",
                AdjustmentMode.SwingBenchmark => "Swing Benchmark",
                AdjustmentMode.Legacy => "Legacy",
                AdjustmentMode.RotationAuto => "Rotation Auto",
                AdjustmentMode.RoomOffset => "Room offset",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}