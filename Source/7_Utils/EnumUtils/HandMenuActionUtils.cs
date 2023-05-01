using System;
using System.Collections.Generic;

namespace EasyOffset {
    public static class HandMenuActionUtils {
        private static readonly Dictionary<string, HandMenuAction> NameToTypeDictionary = new();

        #region Left

        public static readonly object[] LeftHandMenuChoicesObjects;

        private static readonly HandMenuAction[] LeftHandMenuChoices = {
            HandMenuAction.Default,
            HandMenuAction.LeftMirrorAll,
            HandMenuAction.MirrorPivotPosition,
            HandMenuAction.MirrorRotation,
            HandMenuAction.MirrorZOffset,
            HandMenuAction.Reset
        };

        #endregion

        #region Right

        public static readonly object[] RightHandMenuChoicesObjects;

        private static readonly HandMenuAction[] RightHandMenuChoices = {
            HandMenuAction.Default,
            HandMenuAction.RightMirrorAll,
            HandMenuAction.MirrorPivotPosition,
            HandMenuAction.MirrorRotation,
            HandMenuAction.MirrorZOffset,
            HandMenuAction.Reset
        };

        #endregion

        #region Constructor

        static HandMenuActionUtils() {
            var allEnumValues = typeof(HandMenuAction).GetEnumValues();

            foreach (HandMenuAction type in allEnumValues) {
                var name = TypeToName(type);
                NameToTypeDictionary.Add(name, type);
            }

            LeftHandMenuChoicesObjects = new object[LeftHandMenuChoices.Length];
            for (var i = 0; i < LeftHandMenuChoices.Length; i++) {
                LeftHandMenuChoicesObjects[i] = TypeToName(LeftHandMenuChoices[i]);
            }

            RightHandMenuChoicesObjects = new object[RightHandMenuChoices.Length];
            for (var i = 0; i < RightHandMenuChoices.Length; i++) {
                RightHandMenuChoicesObjects[i] = TypeToName(RightHandMenuChoices[i]);
            }
        }

        #endregion

        #region NameToType

        public static HandMenuAction NameToTypeOrDefault(string name) {
            return NameToTypeDictionary.ContainsKey(name) ? NameToTypeDictionary[name] : HandMenuAction.Default;
        }

        public static HandMenuAction NameToType(string name) {
            return NameToTypeDictionary[name];
        }

        #endregion

        #region TypeToName

        public static string TypeToName(HandMenuAction type) {
            return type switch {
                HandMenuAction.Default => "Select action",
                HandMenuAction.LeftMirrorAll => "Mirror to Right",
                HandMenuAction.RightMirrorAll => "Mirror to Left",
                HandMenuAction.MirrorPivotPosition => "Mirror position",
                HandMenuAction.MirrorRotation => "Mirror rotation",
                HandMenuAction.MirrorZOffset => "Mirror ZOffset",
                HandMenuAction.Reset => "Reset to defaults",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}