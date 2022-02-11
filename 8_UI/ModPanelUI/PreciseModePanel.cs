using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Active

    private bool _precisePanelActive;

    [UIValue("precise-panel-active")]
    [UsedImplicitly]
    private bool PrecisePanelActive {
        get => _precisePanelActive;
        set {
            _precisePanelActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Panel Scaling

    [UIComponent("precise-panel-component")] [UsedImplicitly]
    private RectTransform _precisePanelComponent;

    private void ApplyScale() {
        _precisePanelComponent.localScale = Vector3.one * 0.84f;
    }

    #endregion

    #region Events

    private void SubscribeToPreciseModeEvents() {
        PluginConfig.AdjustmentModeChangedEvent += PreciseOnAdjustmentModeChanged;
        UpdatePreciseUI();
    }

    private void PreciseOnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        if (adjustmentMode != AdjustmentMode.Precise) return;
        ApplyScale();
        UpdatePreciseUI();
    }

    private void ApplyPreciseConfig() {
        PluginConfig.ApplyPreciseModeConfig(
            PreciseLeftPosition,
            PreciseLeftRotation,
            PreciseLeftZOffset,
            PreciseRightPosition,
            PreciseRightRotation,
            PreciseRightZOffset
        );
        UpdateZOffsetSliders();
    }

    private void UpdatePreciseUI() {
        PluginConfig.GetPreciseModeConfig(
            out var leftPosition,
            out var leftRotation,
            out var leftZOffset,
            out var rightPosition,
            out var rightRotation,
            out var rightZOffset
        );

        PreciseLeftPosition = leftPosition;
        PreciseLeftRotation = leftRotation;
        PreciseLeftZOffset = leftZOffset;

        PreciseRightPosition = rightPosition;
        PreciseRightRotation = rightRotation;
        PreciseRightZOffset = rightZOffset;
    }

    #endregion

    #region ZOffsetSliderSettings

    [UIValue("z-offset-slider-min")] [UsedImplicitly]
    private float _preciseZOffsetSliderMin = -0.2f;

    [UIValue("z-offset-slider-max")] [UsedImplicitly]
    private float _preciseZOffsetSliderMax = 0.25f;

    [UIValue("z-offset-slider-increment")] [UsedImplicitly]
    private float _preciseZOffsetSliderIncrement = 0.001f;

    [UIAction("z-offset-slider-formatter")]
    [UsedImplicitly]
    private string PreciseZOffsetSliderFormatter(float value) => $"{value * 100f:F1} cm";

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
    private string PosSliderFormatter(float value) => $"{value * 100f:F1} cm";

    #endregion

    #region Rotation slider settings

    [UIValue("rot-x-slider-min")] [UsedImplicitly]
    private float _rotXSliderMin = -89.9f;

    [UIValue("rot-x-slider-max")] [UsedImplicitly]
    private float _rotXSliderMax = 89.9f;

    [UIValue("rot-y-slider-min")] [UsedImplicitly]
    private float _rotYSliderMin = -180.0f;

    [UIValue("rot-y-slider-max")] [UsedImplicitly]
    private float _rotYSliderMax = 180.0f;

    [UIValue("rot-slider-increment")] [UsedImplicitly]
    private float _rotSliderIncrement = 0.1f;

    [UIAction("rot-slider-formatter")]
    [UsedImplicitly]
    private string RotSliderFormatter(float value) => $"{value:F1}°";

    #endregion

    #region Mirror

    [UIAction("precise-mirror-to-right-on-click")]
    [UsedImplicitly]
    private void PreciseMirrorToRightOnClick() {
        PreciseRightPosX = -PreciseLeftPosX;
        PreciseRightPosY = PreciseLeftPosY;
        PreciseRightPosZ = PreciseLeftPosZ;
        PreciseRightRotX = PreciseLeftRotX;
        PreciseRightRotY = -PreciseLeftRotY;
        PreciseRightZOffset = PreciseLeftZOffset;
        ApplyPreciseConfig();
    }

    [UIAction("precise-mirror-to-left-on-click")]
    [UsedImplicitly]
    private void PreciseMirrorToLeftOnClick() {
        PreciseLeftPosX = -PreciseRightPosX;
        PreciseLeftPosY = PreciseRightPosY;
        PreciseLeftPosZ = PreciseRightPosZ;
        PreciseLeftRotX = PreciseRightRotX;
        PreciseLeftRotY = -PreciseRightRotY;
        PreciseLeftZOffset = PreciseRightZOffset;
        ApplyPreciseConfig();
    }

    #endregion

    #region LeftHand

    #region Combined

    private Vector3 PreciseLeftPosition {
        get => new(PreciseLeftPosX, PreciseLeftPosY, PreciseLeftPosZ);
        set {
            PreciseLeftPosX = value.x;
            PreciseLeftPosY = value.y;
            PreciseLeftPosZ = value.z;
        }
    }

    private Vector3 PreciseLeftRotation {
        get => new(PreciseLeftRotX, PreciseLeftRotY, 0.0f);
        set {
            PreciseLeftRotX = value.x;
            PreciseLeftRotY = value.y;
        }
    }

    #endregion

    #region ZOffset

    private float _preciseLeftZOffset;

    [UIValue("precise-left-z-offset-value")]
    [UsedImplicitly]
    private float PreciseLeftZOffset {
        get => _preciseLeftZOffset;
        set {
            _preciseLeftZOffset = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-z-offset-on-change")]
    [UsedImplicitly]
    private void PreciseLeftZOffsetOnChange(float value) {
        _preciseLeftZOffsetTarget = value;
        OnSmoothValueChanged(null);
    }

    [UIAction("precise-left-z-offset-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftZOffsetIncOnClick() {
        var newValue = StepUp(PreciseLeftZOffset, _posSliderIncrement);
        PreciseLeftZOffset = FinalizeZOffsetSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-z-offset-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftZOffsetDecOnClick() {
        var newValue = StepDown(PreciseLeftZOffset, _posSliderIncrement);
        PreciseLeftZOffset = FinalizeZOffsetSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region PositionX

    private float _preciseLeftPosX;

    [UIValue("precise-left-pos-x-value")]
    [UsedImplicitly]
    private float PreciseLeftPosX {
        get => _preciseLeftPosX;
        set {
            _preciseLeftPosX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-pos-x-on-change")]
    [UsedImplicitly]
    private void PreciseLeftPosXOnChange(float value) {
        _preciseLeftPosXTarget = value;
        OnSmoothValueChanged(Hand.Left);
    }

    [UIAction("precise-left-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosXIncOnClick() {
        var newValue = StepUp(PreciseLeftPosX, _posSliderIncrement);
        PreciseLeftPosX = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosXDecOnClick() {
        var newValue = StepDown(PreciseLeftPosX, _posSliderIncrement);
        PreciseLeftPosX = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region PositionY

    private float _preciseLeftPosY;

    [UIValue("precise-left-pos-y-value")]
    [UsedImplicitly]
    private float PreciseLeftPosY {
        get => _preciseLeftPosY;
        set {
            _preciseLeftPosY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-pos-y-on-change")]
    [UsedImplicitly]
    private void PreciseLeftPosYOnChange(float value) {
        _preciseLeftPosYTarget = value;
        OnSmoothValueChanged(Hand.Left);
    }

    [UIAction("precise-left-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosYIncOnClick() {
        var newValue = StepUp(PreciseLeftPosY, _posSliderIncrement);
        PreciseLeftPosY = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosYDecOnClick() {
        var newValue = StepDown(PreciseLeftPosY, _posSliderIncrement);
        PreciseLeftPosY = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region PositionZ

    private float _preciseLeftPosZ;

    [UIValue("precise-left-pos-z-value")]
    [UsedImplicitly]
    private float PreciseLeftPosZ {
        get => _preciseLeftPosZ;
        set {
            _preciseLeftPosZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-pos-z-on-change")]
    [UsedImplicitly]
    private void PreciseLeftPosZOnChange(float value) {
        _preciseLeftPosZTarget = value;
        OnSmoothValueChanged(Hand.Left);
    }

    [UIAction("precise-left-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosZIncOnClick() {
        var newValue = StepUp(PreciseLeftPosZ, _posSliderIncrement);
        PreciseLeftPosZ = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosZDecOnClick() {
        var newValue = StepDown(PreciseLeftPosZ, _posSliderIncrement);
        PreciseLeftPosZ = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region RotationX

    private float _preciseLeftRotX;

    [UIValue("precise-left-rot-x-value")]
    [UsedImplicitly]
    private float PreciseLeftRotX {
        get => _preciseLeftRotX;
        set {
            _preciseLeftRotX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-x-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotXOnChange(float value) {
        _preciseLeftRotXTarget = value;
        OnSmoothValueChanged(Hand.Left);
    }

    [UIAction("precise-left-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotXIncOnClick() {
        var newValue = StepUp(PreciseLeftRotX, _rotSliderIncrement);
        PreciseLeftRotX = FinalizeXRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void FinalizeXRotSliderValue() {
        var newValue = StepDown(PreciseLeftRotX, _rotSliderIncrement);
        PreciseLeftRotX = FinalizeYRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region RotationY

    private float _preciseLeftRotY;

    [UIValue("precise-left-rot-y-value")]
    [UsedImplicitly]
    private float PreciseLeftRotY {
        get => _preciseLeftRotY;
        set {
            _preciseLeftRotY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-y-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotYOnChange(float value) {
        _preciseLeftRotYTarget = value;
        OnSmoothValueChanged(Hand.Left);
    }

    [UIAction("precise-left-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotYIncOnClick() {
        var newValue = StepUp(PreciseLeftRotY, _rotSliderIncrement);
        PreciseLeftRotY = FinalizeYRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotYDecOnClick() {
        var newValue = StepDown(PreciseLeftRotY, _rotSliderIncrement);
        PreciseLeftRotY = FinalizeYRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #endregion

    #region RightHand

    #region Combined

    private Vector3 PreciseRightPosition {
        get => new(PreciseRightPosX, PreciseRightPosY, PreciseRightPosZ);
        set {
            PreciseRightPosX = value.x;
            PreciseRightPosY = value.y;
            PreciseRightPosZ = value.z;
        }
    }

    private Vector3 PreciseRightRotation {
        get => new(PreciseRightRotX, PreciseRightRotY, 0.0f);
        set {
            PreciseRightRotX = value.x;
            PreciseRightRotY = value.y;
        }
    }

    #endregion

    #region ZOffset

    private float _preciseRightZOffset;

    [UIValue("precise-right-z-offset-value")]
    [UsedImplicitly]
    private float PreciseRightZOffset {
        get => _preciseRightZOffset;
        set {
            _preciseRightZOffset = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-z-offset-on-change")]
    [UsedImplicitly]
    private void PreciseRightZOffsetOnChange(float value) {
        _preciseRightZOffsetTarget = value;
        OnSmoothValueChanged(null);
    }

    [UIAction("precise-right-z-offset-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightZOffsetIncOnClick() {
        var newValue = StepUp(PreciseRightZOffset, _posSliderIncrement);
        PreciseRightZOffset = FinalizeZOffsetSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-z-offset-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightZOffsetDecOnClick() {
        var newValue = StepDown(PreciseRightZOffset, _posSliderIncrement);
        PreciseRightZOffset = FinalizeZOffsetSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region PositionX

    private float _preciseRightPosX;

    [UIValue("precise-right-pos-x-value")]
    [UsedImplicitly]
    private float PreciseRightPosX {
        get => _preciseRightPosX;
        set {
            _preciseRightPosX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-pos-x-on-change")]
    [UsedImplicitly]
    private void PreciseRightPosXOnChange(float value) {
        _preciseRightPosXTarget = value;
        OnSmoothValueChanged(Hand.Right);
    }

    [UIAction("precise-right-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosXIncOnClick() {
        var newValue = StepUp(PreciseRightPosX, _posSliderIncrement);
        PreciseRightPosX = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosXDecOnClick() {
        var newValue = StepDown(PreciseRightPosX, _posSliderIncrement);
        PreciseRightPosX = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region PositionY

    private float _preciseRightPosY;

    [UIValue("precise-right-pos-y-value")]
    [UsedImplicitly]
    private float PreciseRightPosY {
        get => _preciseRightPosY;
        set {
            _preciseRightPosY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-pos-y-on-change")]
    [UsedImplicitly]
    private void PreciseRightPosYOnChange(float value) {
        _preciseRightPosYTarget = value;
        OnSmoothValueChanged(Hand.Right);
    }

    [UIAction("precise-right-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosYIncOnClick() {
        var newValue = StepUp(PreciseRightPosY, _posSliderIncrement);
        PreciseRightPosY = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosYDecOnClick() {
        var newValue = StepDown(PreciseRightPosY, _posSliderIncrement);
        PreciseRightPosY = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region PositionZ

    private float _preciseRightPosZ;

    [UIValue("precise-right-pos-z-value")]
    [UsedImplicitly]
    private float PreciseRightPosZ {
        get => _preciseRightPosZ;
        set {
            _preciseRightPosZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-pos-z-on-change")]
    [UsedImplicitly]
    private void PreciseRightPosZOnChange(float value) {
        _preciseRightPosZTarget = value;
        OnSmoothValueChanged(Hand.Right);
    }

    [UIAction("precise-right-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosZIncOnClick() {
        var newValue = StepUp(PreciseRightPosZ, _posSliderIncrement);
        PreciseRightPosZ = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosZDecOnClick() {
        var newValue = StepDown(PreciseRightPosZ, _posSliderIncrement);
        PreciseRightPosZ = FinalizePosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region RotationX

    private float _preciseRightRotX;

    [UIValue("precise-right-rot-x-value")]
    [UsedImplicitly]
    private float PreciseRightRotX {
        get => _preciseRightRotX;
        set {
            _preciseRightRotX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-rot-x-on-change")]
    [UsedImplicitly]
    private void PreciseRightRotXOnChange(float value) {
        _preciseRightRotXTarget = value;
        OnSmoothValueChanged(Hand.Right);
    }

    [UIAction("precise-right-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotXIncOnClick() {
        var newValue = StepUp(PreciseRightRotX, _rotSliderIncrement);
        PreciseRightRotX = FinalizeXRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotXDecOnClick() {
        var newValue = StepDown(PreciseRightRotX, _rotSliderIncrement);
        PreciseRightRotX = FinalizeXRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region RotationY

    private float _preciseRightRotY;

    [UIValue("precise-right-rot-y-value")]
    [UsedImplicitly]
    private float PreciseRightRotY {
        get => _preciseRightRotY;
        set {
            _preciseRightRotY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-rot-y-on-change")]
    [UsedImplicitly]
    private void PreciseRightRotYOnChange(float value) {
        _preciseRightRotYTarget = value;
        OnSmoothValueChanged(Hand.Right);
    }

    [UIAction("precise-right-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotYIncOnClick() {
        var newValue = StepUp(PreciseRightRotY, _rotSliderIncrement);
        PreciseRightRotY = FinalizeYRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotYDecOnClick() {
        var newValue = StepDown(PreciseRightRotY, _rotSliderIncrement);
        PreciseRightRotY = FinalizeYRotSliderValue(newValue);
        ApplyPreciseConfig();
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

    private float FinalizeZOffsetSliderValue(float value) {
        return Mathf.Clamp(value, _zOffsetSliderMin, _zOffsetSliderMax);
    }
    
    private float FinalizePosSliderValue(float value) {
        return Mathf.Clamp(value, _posSliderMin, _posSliderMax);
    }

    private float FinalizeXRotSliderValue(float value) {
        return Mathf.Clamp(value, _rotXSliderMin, _rotXSliderMax);
    }

    private static float FinalizeYRotSliderValue(float value) {
        return value switch {
            >= 180 => -360 + value,
            <= -180 => 360 - value,
            _ => value
        };
    }

    #endregion
}