namespace EasyOffset.Configuration {
    internal static class Defaults {
        #region DisplayControllerType

        public static readonly string DisplayControllerType = ControllerTypeUtils.TypeToName(ControllerType.None);

        #endregion

        #region DrawGizmos

        public const bool UseFreeHand = false;

        #endregion

        #region Offsets

        public const float PivotX = 0f;
        public const float PivotY = 0f;
        public const float PivotZ = -0.11f;

        public const float SaberDirectionX = 0f;
        public const float SaberDirectionY = 0f;
        public const float SaberDirectionZ = 1f;

        public const float ZOffset = 0.0f;

        #endregion
    }
}