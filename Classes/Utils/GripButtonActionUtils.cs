using System;
using System.Collections.Generic;

namespace EasyOffset {
    public static class GripButtonActionUtils {
        private static readonly Dictionary<string, GripButtonAction> NameToTypeDictionary = new();
        public static readonly object[] AllNamesObjects;

        #region Constructor

        static GripButtonActionUtils() {
            var allEnumValues = typeof(GripButtonAction).GetEnumValues();

            AllNamesObjects = new object[allEnumValues.Length];
            var index = 0;

            foreach (GripButtonAction type in allEnumValues) {
                var name = TypeToName(type);
                NameToTypeDictionary.Add(name, type);
                AllNamesObjects[index++] = name;
            }
        }

        #endregion

        #region NameToType

        public static GripButtonAction NameToTypeOrNone(string name) {
            return NameToTypeDictionary.ContainsKey(name) ? NameToTypeDictionary[name] : GripButtonAction.None;
        }

        public static GripButtonAction NameToType(string name) {
            return NameToTypeDictionary[name];
        }

        #endregion

        #region TypeToName

        public static string TypeToName(GripButtonAction type) {
            return type switch {
                GripButtonAction.None => "None",
                GripButtonAction.FullGrip => "Full Grip",
                GripButtonAction.PivotOnly => "Pivot Only",
                GripButtonAction.DirectionOnly => "Direction Only",
                GripButtonAction.RoomOffset => "Room offset",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}