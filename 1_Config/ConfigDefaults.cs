using UnityEngine;

namespace EasyOffset {
    internal static class ConfigDefaults {
        #region Enabled

        public const bool Enabled = true;

        #endregion

        #region HideControllers

        public const bool HideControllers = true;

        #endregion

        #region MinimalWarningLevel

        public static readonly string MinimalWarningLevel = WarningLevelUtils.TypeToName(WarningLevel.NonCritical);

        #endregion

        #region RoomOffset

        public const bool AllowRoomXChange = true;
        public const bool AllowRoomYChange = true;
        public const bool AllowRoomZChange = true;

        #endregion

        #region UI Lock

        public const bool UILock = false;

        #endregion

        #region AssignedButton

        public static readonly string AssignedButton = ControllerButtonUtils.TypeToName(ControllerButtonUtils.GetDefaultButton(ControllerType.None));

        #endregion

        #region DisplayControllerType

        public static readonly string DisplayControllerType = ControllerTypeUtils.TypeToName(ControllerType.None);

        #endregion

        #region UseFreeHand

        public const bool UseFreeHand = false;

        #endregion

        #region SaberOffsets

        public const float ZOffset = 0.11f;

        public static readonly Vector3 LeftSaberPivotPosition = new(0.0f, 0.0f, -ZOffset);
        public static readonly Quaternion LeftSaberRotation = Quaternion.identity;

        public static readonly Vector3 RightSaberPivotPosition = new(0.0f, 0.0f, -ZOffset);
        public static readonly Quaternion RightSaberRotation = Quaternion.identity;

        #endregion
    }
}