using System;
using System.Collections.Generic;
using UnityEngine;

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

        public static ControllerType NameToTypeOrNone(string name) {
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
                ControllerType.ViveTracker2 => "Vive Tracker 2.0",
                ControllerType.ViveTracker3 => "Vive Tracker 3.0",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion

        #region Prefabs

        public static GameObject GetRightControllerPrefab(ControllerType controllerType) {
            return controllerType switch {
                ControllerType.ValveIndex => BundleLoader.ValveIndexRight,
                ControllerType.OculusQuest2 => BundleLoader.OculusQuest2Right,
                ControllerType.OculusRiftS => BundleLoader.RiftSRight,
                ControllerType.OculusCV1 => BundleLoader.OculusCV1Right,
                ControllerType.HtcVive => BundleLoader.Vive,
                ControllerType.ViveTracker2 => BundleLoader.ViveTracker2,
                ControllerType.ViveTracker3 => BundleLoader.ViveTracker3,

                ControllerType.None => throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null),
                _ => throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null)
            };
        }

        public static GameObject GetLeftControllerPrefab(ControllerType controllerType) {
            return controllerType switch {
                ControllerType.ValveIndex => BundleLoader.ValveIndexLeft,
                ControllerType.OculusQuest2 => BundleLoader.OculusQuest2Left,
                ControllerType.OculusRiftS => BundleLoader.RiftSLeft,
                ControllerType.OculusCV1 => BundleLoader.OculusCV1Left,
                ControllerType.HtcVive => BundleLoader.Vive,
                ControllerType.ViveTracker2 => BundleLoader.ViveTracker2,
                ControllerType.ViveTracker3 => BundleLoader.ViveTracker3,

                ControllerType.None => throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null),
                _ => throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null)
            };
        }

        #endregion
    }
}