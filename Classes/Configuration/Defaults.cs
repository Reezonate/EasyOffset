namespace EasyOffset.Configuration {
    internal static class Defaults {
        #region UI Lock

        public static readonly bool UILock = false;

        #endregion
        
        #region AssignedButton

        public static readonly string AssignedButton = ControllerButtonUtils.TypeToName(ControllerButton.Grip);

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