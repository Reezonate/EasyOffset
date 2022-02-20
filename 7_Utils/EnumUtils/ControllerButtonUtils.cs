using System;
using System.Collections.Generic;

namespace EasyOffset {
    internal static class ControllerButtonUtils {
        private static readonly Dictionary<string, ControllerButton> NameToTypeDictionary = new();

        #region Constructor

        static ControllerButtonUtils() {
            var allEnumValues = typeof(ControllerButton).GetEnumValues();

            foreach (ControllerButton type in allEnumValues) {
                var name = TypeToName(type);
                NameToTypeDictionary.Add(name, type);
            }
        }

        #endregion

        #region Button-Alias Dictionaries

        private static readonly Dictionary<ControllerButton, string> OculusButtons = new() {
            [ControllerButton.GripButton] = "Grip button",
            [ControllerButton.PrimaryButton] = "Y/B button",
            [ControllerButton.SecondaryButton] = "X/A button",
            [ControllerButton.Primary2DAxisClick] = "Joystick click"
        };

        private static readonly Dictionary<ControllerButton, string> ValveIndexButtons = new() {
            [ControllerButton.PrimaryButton] = "System button",
            [ControllerButton.SecondaryButton] = "B button",
            [ControllerButton.Primary2DAxisClick] = "Joystick click"
        };

        private static readonly Dictionary<ControllerButton, string> ViveButtons = new() {
            [ControllerButton.GripButton] = "Grip button",
            [ControllerButton.PrimaryButton] = "Menu button",
            [ControllerButton.Primary2DAxisClick] = "Trackpad click"
        };

        private static readonly Dictionary<ControllerButton, string> DefaultButtons = new() {
            [ControllerButton.GripButton] = "Grip button",
            [ControllerButton.PrimaryButton] = "Primary button",
            [ControllerButton.SecondaryButton] = "Secondary button",
            [ControllerButton.Primary2DAxisClick] = "Joystick click"
        };

        #endregion

        #region GetDefaultButton

        public static ControllerButton GetDefaultButton(ControllerType controllerType) {
            switch (controllerType) {
                case ControllerType.ValveIndex:
                    return ControllerButton.SecondaryButton;

                case ControllerType.None:
                case ControllerType.OculusQuest2:
                case ControllerType.OculusRiftS:
                case ControllerType.OculusCV1:
                case ControllerType.HtcVive:
                case ControllerType.ViveTracker2:
                case ControllerType.ViveTracker3:
                    return ControllerButton.GripButton;

                default: throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
            }
        }

        #endregion

        #region GetAvailableOptions

        public static Dictionary<ControllerButton, string> GetAvailableOptions(ControllerType controllerType) {
            switch (controllerType) {
                case ControllerType.None: return DefaultButtons;
                case ControllerType.ValveIndex: return ValveIndexButtons;

                case ControllerType.OculusQuest2:
                case ControllerType.OculusRiftS:
                case ControllerType.OculusCV1: return OculusButtons;

                case ControllerType.HtcVive:
                case ControllerType.ViveTracker2:
                case ControllerType.ViveTracker3: return ViveButtons;

                default: throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
            }
        }

        #endregion

        #region NameToType

        public static ControllerButton NameToTypeOrDefault(ControllerType controllerType, string name) {
            return NameToTypeDictionary.ContainsKey(name) ? NameToTypeDictionary[name] : GetDefaultButton(controllerType);
        }

        public static ControllerButton NameToType(string name) {
            return NameToTypeDictionary[name];
        }

        #endregion

        #region TypeToName

        public static string TypeToName(ControllerButton type) {
            return type switch {
                ControllerButton.TriggerButton => "TriggerButton",
                ControllerButton.PrimaryButton => "PrimaryButton",
                ControllerButton.SecondaryButton => "SecondaryButton",
                ControllerButton.GripButton => "GripButton",
                ControllerButton.Primary2DAxisClick => "Primary2DAxisClick",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}