using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Active

    private bool _legacyPanelActive;

    [UIValue("legacy-panel-active")]
    [UsedImplicitly]
    private bool LegacyPanelActive {
        get => _legacyPanelActive;
        set {
            _legacyPanelActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _legacySettingsActive;

    [UIValue("legacy-settings-active")]
    [UsedImplicitly]
    private bool LegacySettingsActive {
        get => _legacySettingsActive;
        set {
            _legacySettingsActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _legacyDevicelessWarningActive;

    [UIValue("legacy-deviceless-warning-active")]
    [UsedImplicitly]
    private bool LegacyDevicelessWarningActive {
        get => _legacyDevicelessWarningActive;
        set {
            _legacyDevicelessWarningActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Panel Scaling

    [UIComponent("legacy-panel-component")] [UsedImplicitly]
    private RectTransform _legacyPanelComponent;

    private void ApplyScale() {
        _legacyPanelComponent.localScale = Vector3.one * 0.84f;
    }

    #endregion

    #region Events

    private void SubscribeToLegacyModeEvents() {
        PluginConfig.AdjustmentModeChangedEvent += LegacyOnAdjustmentModeChanged;
    }

    private void LegacyOnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        if (adjustmentMode != AdjustmentMode.Legacy) return;
        ApplyScale();
        UpdateLegacyConfig();
        LegacyDevicelessWarningActive = !ConfigMigration.IsMigrationPossible;
        LegacySettingsActive = ConfigMigration.IsMigrationPossible;
    }

    private void OnLegacyConfigChanged() {
        ConfigConversions.FromTailor(
            true,
            ConfigMigration.IsValveController,
            ConfigMigration.IsVRModeOculus,
            PluginConfig.LeftHandZOffset,
            PluginConfig.RightHandZOffset,
            LegacyLeftHandPosition,
            LegacyRightHandPosition,
            LegacyLeftHandRotation,
            LegacyRightHandRotation,
            out var leftPivotPosition,
            out var rightPivotPosition,
            out var leftSaberDirection,
            out var rightSaberDirection
        );

        PluginConfig.LeftHandPivotPosition = leftPivotPosition;
        PluginConfig.RightHandPivotPosition = rightPivotPosition;
        PluginConfig.LeftHandSaberDirection = leftSaberDirection;
        PluginConfig.RightHandSaberDirection = rightSaberDirection;
    }

    private void UpdateLegacyConfig() {
        ConfigConversions.ToTailor(
            true,
            ConfigMigration.IsValveController,
            ConfigMigration.IsVRModeOculus,
            PluginConfig.LeftHandTranslation,
            PluginConfig.LeftHandRotation,
            PluginConfig.RightHandTranslation,
            PluginConfig.RightHandRotation,
            out var gripLeftPosition,
            out var gripRightPosition,
            out var gripLeftRotation,
            out var gripRightRotation
        );

        LegacyLeftHandPosition = gripLeftPosition;
        LegacyRightHandPosition = gripRightPosition;
        LegacyLeftHandRotation = gripLeftRotation;
        LegacyRightHandRotation = gripRightRotation;
    }

    #endregion

    #region Position slider settings

    [UIValue("pos-slider-min")] [UsedImplicitly]
    private float _posSliderMin = -0.15f;

    [UIValue("pos-slider-max")] [UsedImplicitly]
    private float _posSliderMax = 0.15f;

    [UIValue("pos-slider-increment")] [UsedImplicitly]
    private float _posSliderIncrement = 0.001f;

    [UIAction("pos-slider-formatter")]
    [UsedImplicitly]
    private string PosSliderFormatter(float value) => $"{value * 100f:F2} cm";

    #endregion

    #region Rotation slider settings

    [UIValue("rot-slider-min")] [UsedImplicitly]
    private float _rotSliderMin = -180.0f;

    [UIValue("rot-slider-max")] [UsedImplicitly]
    private float _rotSliderMax = 180.0f;

    [UIValue("rot-slider-increment")] [UsedImplicitly]
    private float _rotSliderIncrement = 0.5f;

    [UIAction("rot-slider-formatter")]
    [UsedImplicitly]
    private string RotSliderFormatter(float value) => $"{value:F2}°";

    #endregion

    #region Mirror

    [UIAction("legacy-mirror-to-right-on-click")]
    [UsedImplicitly]
    private void LegacyMirrorToRightOnClick() {
        LegacyRightPosX = -LegacyLeftPosX;
        LegacyRightPosY = LegacyLeftPosY;
        LegacyRightPosZ = LegacyLeftPosZ;
        LegacyRightRotX = LegacyLeftRotX;
        LegacyRightRotY = -LegacyLeftRotY;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-mirror-to-left-on-click")]
    [UsedImplicitly]
    private void LegacyMirrorToLeftOnClick() {
        LegacyLeftPosX = -LegacyRightPosX;
        LegacyLeftPosY = LegacyRightPosY;
        LegacyLeftPosZ = LegacyRightPosZ;
        LegacyLeftRotX = LegacyRightRotX;
        LegacyLeftRotY = -LegacyRightRotY;
        OnLegacyConfigChanged();
    }

    #endregion

    #region LeftHand

    private Vector3 LegacyLeftHandPosition {
        get => new(LegacyLeftPosX, LegacyLeftPosY, LegacyLeftPosZ);
        set {
            LegacyLeftPosX = value.x;
            LegacyLeftPosY = value.y;
            LegacyLeftPosZ = value.z;
        }
    }

    private Vector3 LegacyLeftHandRotation {
        get => new(LegacyLeftRotX, LegacyLeftRotY, 0.0f);
        set {
            LegacyLeftRotX = value.x;
            LegacyLeftRotY = value.y;
        }
    }

    #region PositionX

    private float _legacyLeftPosX;

    [UIValue("legacy-left-pos-x-value")]
    [UsedImplicitly]
    private float LegacyLeftPosX {
        get => _legacyLeftPosX;
        set {
            _legacyLeftPosX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-left-pos-x-on-change")]
    [UsedImplicitly]
    private void LegacyLeftPosXOnChange(float value) {
        _legacyLeftPosX = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void LegacyLeftPosXIncOnClick() {
        var newValue = StepUp(LegacyLeftPosX, _posSliderIncrement);
        LegacyLeftPosX = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void LegacyLeftPosXDecOnClick() {
        var newValue = StepDown(LegacyLeftPosX, _posSliderIncrement);
        LegacyLeftPosX = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #region PositionY

    private float _legacyLeftPosY;

    [UIValue("legacy-left-pos-y-value")]
    [UsedImplicitly]
    private float LegacyLeftPosY {
        get => _legacyLeftPosY;
        set {
            _legacyLeftPosY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-left-pos-y-on-change")]
    [UsedImplicitly]
    private void LegacyLeftPosYOnChange(float value) {
        _legacyLeftPosY = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void LegacyLeftPosYIncOnClick() {
        var newValue = StepUp(LegacyLeftPosY, _posSliderIncrement);
        LegacyLeftPosY = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void LegacyLeftPosYDecOnClick() {
        var newValue = StepDown(LegacyLeftPosY, _posSliderIncrement);
        LegacyLeftPosY = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #region PositionZ

    private float _legacyLeftPosZ;

    [UIValue("legacy-left-pos-z-value")]
    [UsedImplicitly]
    private float LegacyLeftPosZ {
        get => _legacyLeftPosZ;
        set {
            _legacyLeftPosZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-left-pos-z-on-change")]
    [UsedImplicitly]
    private void LegacyLeftPosZOnChange(float value) {
        _legacyLeftPosZ = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void LegacyLeftPosZIncOnClick() {
        var newValue = StepUp(LegacyLeftPosZ, _posSliderIncrement);
        LegacyLeftPosZ = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void LegacyLeftPosZDecOnClick() {
        var newValue = StepDown(LegacyLeftPosZ, _posSliderIncrement);
        LegacyLeftPosZ = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #region RotationX

    private float _legacyLeftRotX;

    [UIValue("legacy-left-rot-x-value")]
    [UsedImplicitly]
    private float LegacyLeftRotX {
        get => _legacyLeftRotX;
        set {
            _legacyLeftRotX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-left-rot-x-on-change")]
    [UsedImplicitly]
    private void LegacyLeftRotXOnChange(float value) {
        _legacyLeftRotX = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void LegacyLeftRotXIncOnClick() {
        var newValue = StepUp(LegacyLeftRotX, _rotSliderIncrement);
        LegacyLeftRotX = FinalizeRotSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void LegacyLeftRotXDecOnClick() {
        var newValue = StepDown(LegacyLeftRotX, _rotSliderIncrement);
        LegacyLeftRotX = FinalizeRotSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #region RotationY

    private float _legacyLeftRotY;

    [UIValue("legacy-left-rot-y-value")]
    [UsedImplicitly]
    private float LegacyLeftRotY {
        get => _legacyLeftRotY;
        set {
            _legacyLeftRotY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-left-rot-y-on-change")]
    [UsedImplicitly]
    private void LegacyLeftRotYOnChange(float value) {
        _legacyLeftRotY = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void LegacyLeftRotYIncOnClick() {
        var newValue = StepUp(LegacyLeftRotY, _rotSliderIncrement);
        LegacyLeftRotY = FinalizeRotSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-left-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void LegacyLeftRotYDecOnClick() {
        var newValue = StepDown(LegacyLeftRotY, _rotSliderIncrement);
        LegacyLeftRotY = FinalizeRotSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #endregion

    #region RightHand

    private Vector3 LegacyRightHandPosition {
        get => new(LegacyRightPosX, LegacyRightPosY, LegacyRightPosZ);
        set {
            LegacyRightPosX = value.x;
            LegacyRightPosY = value.y;
            LegacyRightPosZ = value.z;
        }
    }

    private Vector3 LegacyRightHandRotation {
        get => new(LegacyRightRotX, LegacyRightRotY, 0.0f);
        set {
            LegacyRightRotX = value.x;
            LegacyRightRotY = value.y;
        }
    }

    #region PositionX

    private float _legacyRightPosX;

    [UIValue("legacy-right-pos-x-value")]
    [UsedImplicitly]
    private float LegacyRightPosX {
        get => _legacyRightPosX;
        set {
            _legacyRightPosX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-right-pos-x-on-change")]
    [UsedImplicitly]
    private void LegacyRightPosXOnChange(float value) {
        _legacyRightPosX = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void LegacyRightPosXIncOnClick() {
        var newValue = StepUp(LegacyRightPosX, _posSliderIncrement);
        LegacyRightPosX = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void LegacyRightPosXDecOnClick() {
        var newValue = StepDown(LegacyRightPosX, _posSliderIncrement);
        LegacyRightPosX = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #region PositionY

    private float _legacyRightPosY;

    [UIValue("legacy-right-pos-y-value")]
    [UsedImplicitly]
    private float LegacyRightPosY {
        get => _legacyRightPosY;
        set {
            _legacyRightPosY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-right-pos-y-on-change")]
    [UsedImplicitly]
    private void LegacyRightPosYOnChange(float value) {
        _legacyRightPosY = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void LegacyRightPosYIncOnClick() {
        var newValue = StepUp(LegacyRightPosY, _posSliderIncrement);
        LegacyRightPosY = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void LegacyRightPosYDecOnClick() {
        var newValue = StepDown(LegacyRightPosY, _posSliderIncrement);
        LegacyRightPosY = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #region PositionZ

    private float _legacyRightPosZ;

    [UIValue("legacy-right-pos-z-value")]
    [UsedImplicitly]
    private float LegacyRightPosZ {
        get => _legacyRightPosZ;
        set {
            _legacyRightPosZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-right-pos-z-on-change")]
    [UsedImplicitly]
    private void LegacyRightPosZOnChange(float value) {
        _legacyRightPosZ = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void LegacyRightPosZIncOnClick() {
        var newValue = StepUp(LegacyRightPosZ, _posSliderIncrement);
        LegacyRightPosZ = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void LegacyRightPosZDecOnClick() {
        var newValue = StepDown(LegacyRightPosZ, _posSliderIncrement);
        LegacyRightPosZ = FinalizePosSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #region RotationX

    private float _legacyRightRotX;

    [UIValue("legacy-right-rot-x-value")]
    [UsedImplicitly]
    private float LegacyRightRotX {
        get => _legacyRightRotX;
        set {
            _legacyRightRotX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-right-rot-x-on-change")]
    [UsedImplicitly]
    private void LegacyRightRotXOnChange(float value) {
        _legacyRightRotX = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void LegacyRightRotXIncOnClick() {
        var newValue = StepUp(LegacyRightRotX, _rotSliderIncrement);
        LegacyRightRotX = FinalizeRotSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void LegacyRightRotXDecOnClick() {
        var newValue = StepDown(LegacyRightRotX, _rotSliderIncrement);
        LegacyRightRotX = FinalizeRotSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #region RotationY

    private float _legacyRightRotY;

    [UIValue("legacy-right-rot-y-value")]
    [UsedImplicitly]
    private float LegacyRightRotY {
        get => _legacyRightRotY;
        set {
            _legacyRightRotY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("legacy-right-rot-y-on-change")]
    [UsedImplicitly]
    private void LegacyRightRotYOnChange(float value) {
        _legacyRightRotY = value;
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void LegacyRightRotYIncOnClick() {
        var newValue = StepUp(LegacyRightRotY, _rotSliderIncrement);
        LegacyRightRotY = FinalizeRotSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    [UIAction("legacy-right-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void LegacyRightRotYDecOnClick() {
        var newValue = StepDown(LegacyRightRotY, _rotSliderIncrement);
        LegacyRightRotY = FinalizeRotSliderValue(newValue);
        OnLegacyConfigChanged();
    }

    #endregion

    #endregion

    #region Utils

    private static float StepUp(float currentValue, float stepSize) {
        var currentStep = Mathf.RoundToInt(currentValue / stepSize);
        return stepSize * (currentStep + 1);
    }

    private static float StepDown(float currentValue, float stepSize) {
        var currentStep = Mathf.RoundToInt(currentValue / stepSize);
        return stepSize * (currentStep - 1);
    }

    private float FinalizePosSliderValue(float value) {
        return Mathf.Clamp(value, _posSliderMin, _posSliderMax);
    }

    private static float FinalizeRotSliderValue(float value) {
        return value switch {
            >= 180 => -360 + value,
            <= -180 => 360 - value,
            _ => value
        };
    }

    #endregion
}