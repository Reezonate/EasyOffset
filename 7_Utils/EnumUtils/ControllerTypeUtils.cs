using System;
using System.Collections.Generic;

namespace EasyOffset {
    public static class ControllerTypeUtils {
        private static readonly Dictionary<string, ControllerType> NameToTypeDictionary = new();
        public static readonly object[] AllNamesObjects;

        #region Constructor

        static ControllerTypeUtils() {
            var allEnumValues = typeof(ControllerType).GetEnumValues();

            AllNamesObjects = new object[allEnumValues.Length];
            var index = 0;

            foreach (ControllerType type in allEnumValues) {
                var name = TypeToName(type);
                NameToTypeDictionary.Add(name, type);
                AllNamesObjects[index++] = name;
            }
        }

        #endregion

        #region NameToType

        public static ControllerType NameToTypeOrDefault(string name) {
            return NameToTypeDictionary.ContainsKey(name) ? NameToTypeDictionary[name] : ControllerType.None;
        }

        public static ControllerType NameToType(string name) {
            return NameToTypeDictionary[name];
        }

        #endregion

        #region TypeToName

        public static string TypeToName(ControllerType type) {
            return type switch {
                ControllerType.None => "None",
                ControllerType.ValveIndex => "Valve Index",
                ControllerType.OculusQuest2 => "Quest 2",
                ControllerType.OculusRiftS => "RiftS/Quest",
                ControllerType.OculusCV1 => "CV1",
                ControllerType.HtcVive => "HTC Vive",
                ControllerType.PiMaxSword => "PiMax Sword",
                ControllerType.ViveTracker2 => "Vive Tracker 2.0",
                ControllerType.ViveTracker3 => "Vive Tracker 3.0",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}