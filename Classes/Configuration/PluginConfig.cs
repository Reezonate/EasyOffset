using System;
using UnityEngine;

namespace EasyOffset.Configuration {
    public static class PluginConfig {
        #region Smoothing

        public static bool SmoothingEnabled { get; private set; }
        public static float PositionalSmoothing { get; set; }
        public static float RotationalSmoothing { get; set; }
        public static MainSettingsModelSO MainSettingsModel { get; private set; }

        public static void EnableSmoothing(
            MainSettingsModelSO mainSettingsModel
        ) {
            SmoothingEnabled = true;
            MainSettingsModel = mainSettingsModel;
        }

        public static void DisableSmoothing() {
            SmoothingEnabled = false;
        }

        #endregion

        #region Enabled

        public delegate void OnEnabledChangeDelegate(bool newChanged);

        public static event OnEnabledChangeDelegate OnEnabledChange;

        private static readonly CachedVariable<bool> CachedEnabled = new(
            () => ConfigFileData.Instance.Enabled
        );

        public static bool Enabled {
            get => CachedEnabled.Value;
            set {
                CachedEnabled.Value = value;
                ConfigFileData.Instance.Enabled = value;
                OnEnabledChange?.Invoke(value);
            }
        }

        #endregion

        #region UI Lock

        private static readonly CachedVariable<bool> CachedUILock = new(
            () => ConfigFileData.Instance.UILock
        );

        public static bool UILock {
            get => CachedUILock.Value;
            set {
                CachedUILock.Value = value;
                ConfigFileData.Instance.UILock = value;
            }
        }

        #endregion

        #region AssignedButton

        private static readonly CachedVariable<ControllerButton> CachedAssignedButton = new(
            () => ControllerButtonUtils.NameToTypeOrDefault(ConfigFileData.Instance.AssignedButton)
        );

        public static ControllerButton AssignedButton {
            get => CachedAssignedButton.Value;
            set {
                CachedAssignedButton.Value = value;
                ConfigFileData.Instance.AssignedButton = ControllerButtonUtils.TypeToName(value);
            }
        }

        #endregion

        #region AdjustmentMode

        public static event Action<AdjustmentMode> AdjustmentModeChangedEvent;

        private static AdjustmentMode _adjustmentMode = AdjustmentMode.None;

        public static AdjustmentMode AdjustmentMode {
            get => _adjustmentMode;
            set {
                _adjustmentMode = value;
                AdjustmentModeChangedEvent?.Invoke(value);
            }
        }

        #endregion

        #region DisplayController

        public static event Action<ControllerType> ControllerTypeChangedEvent;
        private static bool _hideController;

        private static readonly CachedVariable<ControllerType> CachedDisplayControllerType = new(
            () => ControllerTypeUtils.NameToTypeOrDefault(ConfigFileData.Instance.DisplayControllerType)
        );

        public static ControllerType DisplayControllerType {
            get => _hideController ? ControllerType.None : CachedDisplayControllerType.Value;
            set {
                CachedDisplayControllerType.Value = value;
                ConfigFileData.Instance.DisplayControllerType = ControllerTypeUtils.TypeToName(value);
                ControllerTypeChangedEvent?.Invoke(value);
            }
        }

        public static void HideController() {
            _hideController = true;
            ControllerTypeChangedEvent?.Invoke(ControllerType.None);
        }

        public static void ShowController() {
            _hideController = false;
            ControllerTypeChangedEvent?.Invoke(DisplayControllerType);
        }

        #endregion

        #region EnableMidPlayAdjustment

        public static bool EnableMidPlayAdjustment { get; set; } = false;

        #endregion

        #region UseFreeHand

        private static readonly CachedVariable<bool> CachedUseFreeHand = new(
            () => ConfigFileData.Instance.UseFreeHand
        );

        public static bool UseFreeHand {
            get => CachedUseFreeHand.Value;
            set {
                CachedUseFreeHand.Value = value;
                ConfigFileData.Instance.UseFreeHand = value;
            }
        }

        #endregion

        #region RightHand

        #region Translation

        private static readonly CachedVariable<Vector3> CachedRightHandTranslation = new(GetRightHandTranslationValue);

        public static Vector3 RightHandTranslation => CachedRightHandTranslation.Value;

        private static void UpdateRightHandTranslation() {
            CachedRightHandTranslation.Value = GetRightHandTranslationValue();
        }

        private static Vector3 GetRightHandTranslationValue() {
            return TransformUtils.GetSaberLocalPosition(RightHandPivotPosition, RightHandSaberDirection, RightHandZOffset);
        }

        #endregion

        #region Rotation

        private static readonly CachedVariable<Quaternion> CachedRightHandRotation = new(GetRightHandRotationValue);

        public static Quaternion RightHandRotation => CachedRightHandRotation.Value;

        private static void UpdateRightHandRotation() {
            CachedRightHandRotation.Value = GetRightHandRotationValue();
        }

        private static Quaternion GetRightHandRotationValue() {
            return TransformUtils.GetSaberLocalRotation(RightHandSaberDirection);
        }

        #endregion

        #region PivotPosition

        private static readonly CachedVariable<Vector3> CachedRightHandPivotPosition = new(() => new Vector3(
            ConfigFileData.Instance.RightHandPivotX,
            ConfigFileData.Instance.RightHandPivotY,
            ConfigFileData.Instance.RightHandPivotZ
        ));

        public static Vector3 RightHandPivotPosition {
            get => CachedRightHandPivotPosition.Value;
            set {
                CachedRightHandPivotPosition.Value = value;
                ConfigFileData.Instance.RightHandPivotX = value.x;
                ConfigFileData.Instance.RightHandPivotY = value.y;
                ConfigFileData.Instance.RightHandPivotZ = value.z;
                UpdateRightHandTranslation();
            }
        }

        #endregion

        #region SaberDirection

        private static readonly CachedVariable<Vector3> CachedRightHandSaberDirection = new(() => new Vector3(
            ConfigFileData.Instance.RightHandSaberDirectionX,
            ConfigFileData.Instance.RightHandSaberDirectionY,
            ConfigFileData.Instance.RightHandSaberDirectionZ
        ).normalized);

        public static Vector3 RightHandSaberDirection {
            get => CachedRightHandSaberDirection.Value;
            set {
                var normalized = value.normalized;
                CachedRightHandSaberDirection.Value = normalized;
                ConfigFileData.Instance.RightHandSaberDirectionX = normalized.x;
                ConfigFileData.Instance.RightHandSaberDirectionY = normalized.y;
                ConfigFileData.Instance.RightHandSaberDirectionZ = normalized.z;
                UpdateRightHandTranslation();
                UpdateRightHandRotation();
            }
        }

        #endregion

        #region ZOffset

        private static readonly CachedVariable<float> CachedRightHandZOffset = new(
            () => ConfigFileData.Instance.RightHandZOffset
        );

        public static float RightHandZOffset {
            get => CachedRightHandZOffset.Value;
            set {
                CachedRightHandZOffset.Value = value;
                ConfigFileData.Instance.RightHandZOffset = value;
                UpdateRightHandTranslation();
            }
        }

        #endregion

        #endregion

        #region LeftHand

        #region Translation

        private static readonly CachedVariable<Vector3> CachedLeftHandTranslation = new(GetLeftHandTranslationValue);

        public static Vector3 LeftHandTranslation => CachedLeftHandTranslation.Value;

        private static void UpdateLeftHandTranslation() {
            CachedLeftHandTranslation.Value = GetLeftHandTranslationValue();
        }

        private static Vector3 GetLeftHandTranslationValue() {
            return TransformUtils.GetSaberLocalPosition(LeftHandPivotPosition, LeftHandSaberDirection, LeftHandZOffset);
        }

        #endregion

        #region Rotation

        private static readonly CachedVariable<Quaternion> CachedLeftHandRotation = new(GetLeftHandRotationValue);

        public static Quaternion LeftHandRotation => CachedLeftHandRotation.Value;

        private static void UpdateLeftHandRotation() {
            CachedLeftHandRotation.Value = GetLeftHandRotationValue();
        }

        private static Quaternion GetLeftHandRotationValue() {
            return TransformUtils.GetSaberLocalRotation(LeftHandSaberDirection);
        }

        #endregion

        #region PivotPosition

        private static readonly CachedVariable<Vector3> CachedLeftHandPivotPosition = new(() => new Vector3(
            ConfigFileData.Instance.LeftHandPivotX,
            ConfigFileData.Instance.LeftHandPivotY,
            ConfigFileData.Instance.LeftHandPivotZ
        ));

        public static Vector3 LeftHandPivotPosition {
            get => CachedLeftHandPivotPosition.Value;
            set {
                CachedLeftHandPivotPosition.Value = value;
                ConfigFileData.Instance.LeftHandPivotX = value.x;
                ConfigFileData.Instance.LeftHandPivotY = value.y;
                ConfigFileData.Instance.LeftHandPivotZ = value.z;
                UpdateLeftHandTranslation();
            }
        }

        #endregion

        #region SaberDirection

        private static readonly CachedVariable<Vector3> CachedLeftHandSaberDirection = new(() => new Vector3(
            ConfigFileData.Instance.LeftHandSaberDirectionX,
            ConfigFileData.Instance.LeftHandSaberDirectionY,
            ConfigFileData.Instance.LeftHandSaberDirectionZ
        ).normalized);

        public static Vector3 LeftHandSaberDirection {
            get => CachedLeftHandSaberDirection.Value;
            set {
                var normalized = value.normalized;
                CachedLeftHandSaberDirection.Value = normalized;
                ConfigFileData.Instance.LeftHandSaberDirectionX = normalized.x;
                ConfigFileData.Instance.LeftHandSaberDirectionY = normalized.y;
                ConfigFileData.Instance.LeftHandSaberDirectionZ = normalized.z;
                UpdateLeftHandTranslation();
                UpdateLeftHandRotation();
            }
        }

        #endregion

        #region ZOffset

        private static readonly CachedVariable<float> CachedLeftHandZOffset = new(
            () => ConfigFileData.Instance.LeftHandZOffset
        );

        public static float LeftHandZOffset {
            get => CachedLeftHandZOffset.Value;
            set {
                CachedLeftHandZOffset.Value = value;
                ConfigFileData.Instance.LeftHandZOffset = value;
                UpdateLeftHandTranslation();
            }
        }

        #endregion

        #endregion

        #region Mirror

        public static void MirrorPivot(Hand mirrorSource) {
            Vector3 from;
            switch (mirrorSource) {
                case Hand.Left:
                    from = LeftHandPivotPosition;
                    RightHandPivotPosition = new Vector3(-from.x, from.y, from.z);
                    break;
                case Hand.Right:
                    from = RightHandPivotPosition;
                    LeftHandPivotPosition = new Vector3(-from.x, from.y, from.z);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
            }
        }

        public static void MirrorSaberDirection(Hand mirrorSource) {
            Vector3 from;
            switch (mirrorSource) {
                case Hand.Left:
                    from = LeftHandSaberDirection;
                    RightHandSaberDirection = new Vector3(-from.x, from.y, from.z);
                    break;
                case Hand.Right:
                    from = RightHandSaberDirection;
                    LeftHandSaberDirection = new Vector3(-from.x, from.y, from.z);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
            }
        }

        public static void MirrorZOffset(Hand mirrorSource) {
            switch (mirrorSource) {
                case Hand.Left:
                    RightHandZOffset = LeftHandZOffset;
                    break;
                case Hand.Right:
                    LeftHandZOffset = RightHandZOffset;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
            }
        }

        #endregion

        #region Reset

        public static void ResetOffsets(Hand hand) {
            switch (hand) {
                case Hand.Left:
                    LeftHandPivotPosition = new Vector3(-Defaults.PivotX, Defaults.PivotY, Defaults.PivotZ);
                    LeftHandSaberDirection = new Vector3(-Defaults.SaberDirectionX, Defaults.SaberDirectionY, Defaults.SaberDirectionZ);
                    LeftHandZOffset = Defaults.ZOffset;
                    break;
                case Hand.Right:
                    RightHandPivotPosition = new Vector3(Defaults.PivotX, Defaults.PivotY, Defaults.PivotZ);
                    RightHandSaberDirection = new Vector3(Defaults.SaberDirectionX, Defaults.SaberDirectionY, Defaults.SaberDirectionZ);
                    RightHandZOffset = Defaults.ZOffset;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
        }

        #endregion

        #region Save

        public static void SaveConfigFile() {
            ConfigFileData.Instance.Changed();
        }

        #endregion

        #region Preset

        public static void ApplyPreset(IConfigPreset preset) {
            RightHandPivotPosition = preset.RightHandPivotPosition;
            RightHandSaberDirection = preset.RightHandSaberDirection;
            RightHandZOffset = preset.RightHandZOffset;
            LeftHandPivotPosition = preset.LeftHandPivotPosition;
            LeftHandSaberDirection = preset.LeftHandSaberDirection;
            LeftHandZOffset = preset.LeftHandZOffset;
        }

        public static IConfigPreset GeneratePreset() {
            return new ConfigPresetV1(
                DateTimeOffset.Now.ToUnixTimeSeconds(),
                DisplayControllerType,
                RightHandPivotPosition,
                RightHandSaberDirection,
                RightHandZOffset,
                LeftHandPivotPosition,
                LeftHandSaberDirection,
                LeftHandZOffset
            );
        }

        #endregion
    }
}