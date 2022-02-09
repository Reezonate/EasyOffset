namespace EasyOffset {
    internal static class Defaults {
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

        #region Offsets

        public const float PivotX = 0f;
        public const float PivotY = 0f;
        public const float PivotZ = -0.11f;

        public const float SaberDirectionX = 0f;
        public const float SaberDirectionY = 0f;
        public const float SaberDirectionZ = 1f;

        public const float ZOffset = 0.11f;

        #endregion
    }
}