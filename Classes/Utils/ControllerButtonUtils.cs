using System;
using System.Collections.Generic;

namespace EasyOffset {
    public static class ControllerButtonUtils {
        private static readonly Dictionary<string, ControllerButton> NameToTypeDictionary = new();
        public static readonly object[] AllNamesObjects;

        #region Constructor

        static ControllerButtonUtils() {
            var allEnumValues = typeof(ControllerButton).GetEnumValues();

            AllNamesObjects = new object[allEnumValues.Length];
            var index = 0;

            foreach (ControllerButton type in allEnumValues) {
                var name = TypeToName(type);
                NameToTypeDictionary.Add(name, type);
                AllNamesObjects[index++] = name;
            }
        }

        #endregion

        #region NameToType

        public static ControllerButton NameToTypeOrDefault(string name) {
            return NameToTypeDictionary.ContainsKey(name) ? NameToTypeDictionary[name] : ControllerButton.Grip;
        }

        public static ControllerButton NameToType(string name) {
            return NameToTypeDictionary[name];
        }

        #endregion

        #region TypeToName

        public static string TypeToName(ControllerButton type) {
            return type switch {
                ControllerButton.Grip => "Grip Button",
                ControllerButton.Joystick => "Joystick click",
                ControllerButton.Primary => "App/Menu button",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}