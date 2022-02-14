using System;
using BeatSaberMarkupLanguage.Attributes;
using IPA.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Events

    private void SubscribeToPrecisePanelEvents() {
        PluginConfig.ConfigWasChangedEvent += OnConfigWasChanged;
        PluginConfig.IsModPanelVisibleChangedEvent += OnIsModPanelVisibleChanged;
        OnConfigWasChanged();
    }

    private void OnConfigWasChanged() {
        if (_smoothingEnabled) return;
        NotifySynchronizationRequired();
    }

    private void OnIsModPanelVisibleChanged(bool isVisible) {
        if (!isVisible) return;
        SetPrecisePanelState(_precisePanelState);
    }

    #endregion

    #region Synchronization

    private int _synchronizationsRequired;

    private void SynchronizationUpdate() {
        if (_synchronizationsRequired <= 0) return;
        SyncPreciseUIValuesWithConfig();
        _synchronizationsRequired -= 1;
    }

    private void NotifySynchronizationRequired() {
        _synchronizationsRequired = 2; //ISSUE: Slider formatter requires 2 frames after activation (1.18.3)
    }

    private void SyncPreciseUIValuesWithConfig() {
        PreciseLeftPivotPosition = PluginConfig.LeftSaberPivotPosition;
        PreciseLeftRotationEuler = PluginConfig.LeftSaberRotationEuler;
        PreciseLeftZOffset = PluginConfig.LeftSaberZOffset;
        PreciseRightPivotPosition = PluginConfig.RightSaberPivotPosition;
        PreciseRightRotationEuler = PluginConfig.RightSaberRotationEuler;
        PreciseRightZOffset = PluginConfig.RightSaberZOffset;
    }

    #endregion

    #region Apply & Reset

    private void ApplyPreciseConfig() {
        PluginConfig.ApplyPreciseModeValues(
            PreciseLeftPivotPosition,
            PreciseLeftRotationEuler,
            PreciseLeftZOffset,
            PreciseRightPivotPosition,
            PreciseRightRotationEuler,
            PreciseRightZOffset
        );
    }

    private void ResetLeftHandPreciseConfig() {
        switch (_precisePanelState) {
            case PrecisePanelState.Hidden: return;
            case PrecisePanelState.PositionOnly:
                PreciseLeftPivotPosition = Vector3.zero;
                PreciseLeftZOffset = 0.0f;
                break;
            case PrecisePanelState.RotationOnly:
                PreciseLeftRotationEuler = Vector3.zero;
                break;
            case PrecisePanelState.ZOffsetOnly:
            case PrecisePanelState.Full:
                PreciseLeftPivotPosition = Vector3.zero;
                PreciseLeftRotationEuler = Vector3.zero;
                PreciseLeftZOffset = 0.0f;
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        ApplyPreciseConfig();
    }

    private void ResetRightHandPreciseConfig() {
        switch (_precisePanelState) {
            case PrecisePanelState.Hidden: return;
            case PrecisePanelState.PositionOnly:
                PreciseRightPivotPosition = Vector3.zero;
                PreciseRightZOffset = 0.0f;
                break;
            case PrecisePanelState.RotationOnly:
                PreciseRightRotationEuler = Vector3.zero;
                break;
            case PrecisePanelState.ZOffsetOnly:
            case PrecisePanelState.Full:
                PreciseRightPivotPosition = Vector3.zero;
                PreciseRightRotationEuler = Vector3.zero;
                PreciseRightZOffset = 0.0f;
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        ApplyPreciseConfig();
    }

    #endregion

    #region Mirror

    private void PreciseMirrorFromLeft() {
        switch (_precisePanelState) {
            case PrecisePanelState.Hidden: return;
            case PrecisePanelState.PositionOnly:
                PreciseRightZOffset = PreciseLeftZOffset;
                PreciseRightPosX = -PreciseLeftPosX;
                PreciseRightPosY = PreciseLeftPosY;
                PreciseRightPosZ = PreciseLeftPosZ;
                break;
            case PrecisePanelState.RotationOnly:
                PreciseRightRotX = PreciseLeftRotX;
                PreciseRightRotY = -PreciseLeftRotY;
                PreciseRightRotZ = -PreciseLeftRotZ;
                break;
            case PrecisePanelState.ZOffsetOnly:
            case PrecisePanelState.Full:
                PreciseRightZOffset = PreciseLeftZOffset;
                PreciseRightPosX = -PreciseLeftPosX;
                PreciseRightPosY = PreciseLeftPosY;
                PreciseRightPosZ = PreciseLeftPosZ;
                PreciseRightRotX = PreciseLeftRotX;
                PreciseRightRotY = -PreciseLeftRotY;
                PreciseRightRotZ = -PreciseLeftRotZ;
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        ApplyPreciseConfig();
    }

    private void PreciseMirrorFromRight() {
        switch (_precisePanelState) {
            case PrecisePanelState.Hidden: return;
            case PrecisePanelState.PositionOnly:
                PreciseLeftZOffset = PreciseRightZOffset;
                PreciseLeftPosX = -PreciseRightPosX;
                PreciseLeftPosY = PreciseRightPosY;
                PreciseLeftPosZ = PreciseRightPosZ;
                break;
            case PrecisePanelState.RotationOnly:
                PreciseLeftRotX = PreciseRightRotX;
                PreciseLeftRotY = -PreciseRightRotY;
                PreciseLeftRotZ = -PreciseRightRotZ;
                break;
            case PrecisePanelState.ZOffsetOnly:
            case PrecisePanelState.Full:
                PreciseLeftZOffset = PreciseRightZOffset;
                PreciseLeftPosX = -PreciseRightPosX;
                PreciseLeftPosY = PreciseRightPosY;
                PreciseLeftPosZ = PreciseRightPosZ;
                PreciseLeftRotX = PreciseRightRotX;
                PreciseLeftRotY = -PreciseRightRotY;
                PreciseLeftRotZ = -PreciseRightRotZ;
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        ApplyPreciseConfig();
    }

    #endregion

    #region State

    private PrecisePanelState _precisePanelState = PrecisePanelState.Hidden;

    private void SetPrecisePanelState(PrecisePanelState newState) {
        switch (newState) {
            case PrecisePanelState.Hidden:
                PrecisePanelActive = false;
                break;
            case PrecisePanelState.ZOffsetOnly:
                PrecisePanelActive = true;
                PreciseZOffsetActive = true;
                PrecisePositionActive = false;
                PreciseRotationActive = false;
                PreciseSlidersHeight = 8.0f;
                PreciseFillerHeight = 0.0f;
                ApplyScale(0.82f);
                break;
            case PrecisePanelState.PositionOnly:
                PrecisePanelActive = true;
                PreciseZOffsetActive = true;
                PrecisePositionActive = true;
                PreciseRotationActive = false;
                PreciseSlidersHeight = 27.0f;
                PreciseFillerHeight = 0.0f;
                ApplyScale(0.82f);
                break;
            case PrecisePanelState.RotationOnly:
                PrecisePanelActive = true;
                PreciseZOffsetActive = false;
                PrecisePositionActive = false;
                PreciseRotationActive = true;
                PreciseSlidersHeight = 20.0f;
                PreciseFillerHeight = 0.0f;
                ApplyScale(0.82f);
                break;
            case PrecisePanelState.Full:
                PrecisePanelActive = true;
                PreciseZOffsetActive = true;
                PrecisePositionActive = true;
                PreciseRotationActive = true;
                PreciseSlidersHeight = 44.0f;
                PreciseFillerHeight = 22.0f;
                ApplyScale(0.8f);
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        _precisePanelState = newState;
        NotifySynchronizationRequired();
    }

    private enum PrecisePanelState {
        Hidden,
        ZOffsetOnly,
        PositionOnly,
        RotationOnly,
        Full
    }

    #endregion

    #region Active & Filler

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

    private bool _preciseZOffsetActive;

    [UIValue("precise-z-offset-active")]
    [UsedImplicitly]
    private bool PreciseZOffsetActive {
        get => _preciseZOffsetActive;
        set {
            _preciseZOffsetActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _precisePositionActive;

    [UIValue("precise-position-active")]
    [UsedImplicitly]
    private bool PrecisePositionActive {
        get => _precisePositionActive;
        set {
            _precisePositionActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _preciseRotationActive;

    [UIValue("precise-rotation-active")]
    [UsedImplicitly]
    private bool PreciseRotationActive {
        get => _preciseRotationActive;
        set {
            _preciseRotationActive = value;
            NotifyPropertyChanged();
        }
    }

    private float _preciseFillerHeight;

    [UIValue("precise-filler-height")]
    [UsedImplicitly]
    private float PreciseFillerHeight {
        get => _preciseFillerHeight;
        set {
            _preciseFillerHeight = value;
            NotifyPropertyChanged();
        }
    }

    private float _preciseSlidersHeight;

    [UIValue("precise-sliders-section-height")]
    [UsedImplicitly]
    private float PreciseSlidersHeight {
        get => _preciseSlidersHeight;
        set {
            _preciseSlidersHeight = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Panel Scaling

    [UIComponent("precise-panel-component")] [UsedImplicitly]
    private RectTransform _precisePanelComponent;

    private void ApplyScale(float scale) {
        _precisePanelComponent.localScale = Vector3.one * scale;
    }

    [UIValue("precise-sliders-section-pad")] [UsedImplicitly]
    private float _precisePanelSlidersWidth = UnityGame.GameVersion.ToString() switch {
        "1.18.3" => 0.0f,
        _ => 1.0f
    };

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
    private float _rotXSliderMin = -89.5f;

    [UIValue("rot-x-slider-max")] [UsedImplicitly]
    private float _rotXSliderMax = 89.5f;

    [UIValue("rot-slider-min")] [UsedImplicitly]
    private float _rotSliderMin = -180.0f;

    [UIValue("rot-slider-max")] [UsedImplicitly]
    private float _rotSliderMax = 180.0f;

    [UIValue("rot-slider-increment")] [UsedImplicitly]
    private float _rotSliderIncrement = 0.1f;

    [UIAction("rot-slider-formatter")]
    [UsedImplicitly]
    private string RotSliderFormatter(float value) => $"{value:F1}°";

    #endregion

    #region ButtonsSettings

    private const int ButtonPromptDelayMillis = 2000;
    private const float ButtonPromptDelaySeconds = 2.0f;

    private const string ResetButtonIdleText = "Reset";
    private const string ResetButtonPromptText = "<color=#ff5555>Sure?</color>";

    private const string LeftMirrorButtonIdleText = "- Mirror to Right >";
    private const string LeftMirrorButtonPromptText = "<color=#ff5555>-<mspace=1.9em> </mspace>Sure?<mspace=1.9em> </mspace>></color>";

    private const string RightMirrorButtonIdleText = "< Mirror to Left -";
    private const string RightMirrorButtonPromptText = "<color=#ff5555><<mspace=1.67em> </mspace>Sure?<mspace=1.67em> </mspace>-</color>";

    #endregion

    #region LeftHand

    #region Reset Button

    private string _preciseLeftResetText = ResetButtonIdleText;
    private bool _preciseLeftResetClickedOnce;

    [UIValue("precise-left-reset-text")]
    [UsedImplicitly]
    private string PreciseLeftResetText {
        get => _preciseLeftResetText;
        set {
            _preciseLeftResetText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-reset-on-click")]
    [UsedImplicitly]
    private void PreciseLeftResetOnClick() {
        if (_preciseLeftResetClickedOnce) {
            ResetLeftHandPreciseConfig();
            ResetLeftResetButton();
        } else {
            StartCoroutine(AsyncUtils.InvokeWithDelay(ResetLeftResetButton, ButtonPromptDelaySeconds));
            PreciseLeftResetText = ResetButtonPromptText;
            _preciseLeftResetClickedOnce = true;
        }
    }

    private void ResetLeftResetButton() {
        PreciseLeftResetText = ResetButtonIdleText;
        _preciseLeftResetClickedOnce = false;
    }

    #endregion

    #region MirrorButton

    private string _preciseLeftMirrorText = LeftMirrorButtonIdleText;
    private bool _preciseLeftMirrorClickedOnce;

    [UIValue("precise-left-mirror-text")]
    [UsedImplicitly]
    private string PreciseLeftMirrorText {
        get => _preciseLeftMirrorText;
        set {
            _preciseLeftMirrorText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-mirror-on-click")]
    [UsedImplicitly]
    private void PreciseLeftMirrorOnClick() {
        if (_preciseLeftMirrorClickedOnce) {
            PreciseMirrorFromLeft();
            ResetLeftMirrorButton();
        } else {
            StartCoroutine(AsyncUtils.InvokeWithDelay(ResetLeftMirrorButton, ButtonPromptDelaySeconds));
            PreciseLeftMirrorText = LeftMirrorButtonPromptText;
            _preciseLeftMirrorClickedOnce = true;
        }
    }

    private void ResetLeftMirrorButton() {
        PreciseLeftMirrorText = LeftMirrorButtonIdleText;
        _preciseLeftMirrorClickedOnce = false;
    }

    #endregion

    #region Combined

    private Vector3 PreciseLeftPivotPosition {
        get => new(PreciseLeftPosX, PreciseLeftPosY, PreciseLeftPosZ);
        set {
            PreciseLeftPosX = value.x;
            PreciseLeftPosY = value.y;
            PreciseLeftPosZ = value.z;
        }
    }

    private Vector3 PreciseLeftRotationEuler {
        get => new(PreciseLeftRotX, PreciseLeftRotY, PreciseLeftRotZ);
        set {
            PreciseLeftRotX = value.x;
            PreciseLeftRotY = value.y;
            PreciseLeftRotZ = value.z;
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
        if (_smoothingEnabled) {
            _preciseLeftZOffsetTarget = value;
            OnSmoothValueChanged(null);
        } else {
            _preciseLeftZOffset = value;
            ApplyPreciseConfig();
        }
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
        if (_smoothingEnabled) {
            _preciseLeftPosXTarget = value;
            OnSmoothValueChanged(Hand.Left);
        } else {
            _preciseLeftPosX = value;
            ApplyPreciseConfig();
        }
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
        if (_smoothingEnabled) {
            _preciseLeftPosYTarget = value;
            OnSmoothValueChanged(Hand.Left);
        } else {
            _preciseLeftPosY = value;
            ApplyPreciseConfig();
        }
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
        if (_smoothingEnabled) {
            _preciseLeftPosZTarget = value;
            OnSmoothValueChanged(Hand.Left);
        } else {
            _preciseLeftPosZ = value;
            ApplyPreciseConfig();
        }
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
        if (_smoothingEnabled) {
            _preciseLeftRotXTarget = value;
            OnSmoothValueChanged(Hand.Left);
        } else {
            _preciseLeftRotX = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotXIncOnClick() {
        var newValue = StepUp(PreciseLeftRotX, _rotSliderIncrement);
        PreciseLeftRotX = FinalizeRotXSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotXDecOnClick() {
        var newValue = StepDown(PreciseLeftRotX, _rotSliderIncrement);
        PreciseLeftRotX = FinalizeRotXSliderValue(newValue);
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
        if (_smoothingEnabled) {
            _preciseLeftRotYTarget = value;
            OnSmoothValueChanged(Hand.Left);
        } else {
            _preciseLeftRotY = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotYIncOnClick() {
        var newValue = StepUp(PreciseLeftRotY, _rotSliderIncrement);
        PreciseLeftRotY = FinalizeRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotYDecOnClick() {
        var newValue = StepDown(PreciseLeftRotY, _rotSliderIncrement);
        PreciseLeftRotY = FinalizeRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region RotationZ

    private float _preciseLeftRotZ;

    [UIValue("precise-left-rot-z-value")]
    [UsedImplicitly]
    private float PreciseLeftRotZ {
        get => _preciseLeftRotZ;
        set {
            _preciseLeftRotZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-z-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotZOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftRotZTarget = value;
            OnSmoothValueChanged(Hand.Left);
        } else {
            _preciseLeftRotZ = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-rot-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotZIncOnClick() {
        var newValue = StepUp(PreciseLeftRotZ, _rotSliderIncrement);
        PreciseLeftRotZ = FinalizeRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotZDecOnClick() {
        var newValue = StepDown(PreciseLeftRotZ, _rotSliderIncrement);
        PreciseLeftRotZ = FinalizeRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #endregion

    #region RightHand

    #region Reset Button

    private string _preciseRightResetText = ResetButtonIdleText;
    private bool _preciseRightResetClickedOnce;

    [UIValue("precise-right-reset-text")]
    [UsedImplicitly]
    private string PreciseRightResetText {
        get => _preciseRightResetText;
        set {
            _preciseRightResetText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-reset-on-click")]
    [UsedImplicitly]
    private void PreciseRightResetOnClick() {
        if (_preciseRightResetClickedOnce) {
            ResetRightHandPreciseConfig();
            ResetRightResetButton();
        } else {
            StartCoroutine(AsyncUtils.InvokeWithDelay(ResetRightResetButton, ButtonPromptDelaySeconds));
            PreciseRightResetText = ResetButtonPromptText;
            _preciseRightResetClickedOnce = true;
        }
    }

    private void ResetRightResetButton() {
        PreciseRightResetText = ResetButtonIdleText;
        _preciseRightResetClickedOnce = false;
    }

    #endregion

    #region MirrorButton

    private string _preciseRightMirrorText = RightMirrorButtonIdleText;
    private bool _preciseRightMirrorClickedOnce;

    [UIValue("precise-right-mirror-text")]
    [UsedImplicitly]
    private string PreciseRightMirrorText {
        get => _preciseRightMirrorText;
        set {
            _preciseRightMirrorText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-mirror-on-click")]
    [UsedImplicitly]
    private void PreciseRightMirrorOnClick() {
        if (_preciseRightMirrorClickedOnce) {
            PreciseMirrorFromRight();
            ResetRightMirrorButton();
        } else {
            StartCoroutine(AsyncUtils.InvokeWithDelay(ResetRightMirrorButton, ButtonPromptDelaySeconds));
            PreciseRightMirrorText = RightMirrorButtonPromptText;
            _preciseRightMirrorClickedOnce = true;
        }
    }

    private void ResetRightMirrorButton() {
        PreciseRightMirrorText = RightMirrorButtonIdleText;
        _preciseRightMirrorClickedOnce = false;
    }

    #endregion

    #region Combined

    private Vector3 PreciseRightPivotPosition {
        get => new(PreciseRightPosX, PreciseRightPosY, PreciseRightPosZ);
        set {
            PreciseRightPosX = value.x;
            PreciseRightPosY = value.y;
            PreciseRightPosZ = value.z;
        }
    }

    private Vector3 PreciseRightRotationEuler {
        get => new(PreciseRightRotX, PreciseRightRotY, PreciseRightRotZ);
        set {
            PreciseRightRotX = value.x;
            PreciseRightRotY = value.y;
            PreciseRightRotZ = value.z;
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
        if (_smoothingEnabled) {
            _preciseRightZOffsetTarget = value;
            OnSmoothValueChanged(null);
        } else {
            _preciseRightZOffset = value;
            ApplyPreciseConfig();
        }
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
        if (_smoothingEnabled) {
            _preciseRightPosXTarget = value;
            OnSmoothValueChanged(Hand.Right);
        } else {
            _preciseRightPosX = value;
            ApplyPreciseConfig();
        }
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
        if (_smoothingEnabled) {
            _preciseRightPosYTarget = value;
            OnSmoothValueChanged(Hand.Right);
        } else {
            _preciseRightPosY = value;
            ApplyPreciseConfig();
        }
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
        if (_smoothingEnabled) {
            _preciseRightPosZTarget = value;
            OnSmoothValueChanged(Hand.Right);
        } else {
            _preciseRightPosZ = value;
            ApplyPreciseConfig();
        }
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
        if (_smoothingEnabled) {
            _preciseRightRotXTarget = value;
            OnSmoothValueChanged(Hand.Right);
        } else {
            _preciseRightRotX = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-right-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotXIncOnClick() {
        var newValue = StepUp(PreciseRightRotX, _rotSliderIncrement);
        PreciseRightRotX = FinalizeRotXSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotXDecOnClick() {
        var newValue = StepDown(PreciseRightRotX, _rotSliderIncrement);
        PreciseRightRotX = FinalizeRotXSliderValue(newValue);
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
        if (_smoothingEnabled) {
            _preciseRightRotYTarget = value;
            OnSmoothValueChanged(Hand.Right);
        } else {
            _preciseRightRotY = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-right-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotYIncOnClick() {
        var newValue = StepUp(PreciseRightRotY, _rotSliderIncrement);
        PreciseRightRotY = FinalizeRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotYDecOnClick() {
        var newValue = StepDown(PreciseRightRotY, _rotSliderIncrement);
        PreciseRightRotY = FinalizeRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    #endregion

    #region RotationZ

    private float _preciseRightRotZ;

    [UIValue("precise-right-rot-z-value")]
    [UsedImplicitly]
    private float PreciseRightRotZ {
        get => _preciseRightRotZ;
        set {
            _preciseRightRotZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-rot-z-on-change")]
    [UsedImplicitly]
    private void PreciseRightRotZOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseRightRotZTarget = value;
            OnSmoothValueChanged(Hand.Right);
        } else {
            _preciseRightRotZ = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-right-rot-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotZIncOnClick() {
        var newValue = StepUp(PreciseRightRotZ, _rotSliderIncrement);
        PreciseRightRotZ = FinalizeRotSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-right-rot-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotZDecOnClick() {
        var newValue = StepDown(PreciseRightRotZ, _rotSliderIncrement);
        PreciseRightRotZ = FinalizeRotSliderValue(newValue);
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
        return Mathf.Clamp(value, _preciseZOffsetSliderMin, _preciseZOffsetSliderMax);
    }

    private float FinalizePosSliderValue(float value) {
        return Mathf.Clamp(value, _posSliderMin, _posSliderMax);
    }

    private float FinalizeRotXSliderValue(float value) {
        return Mathf.Clamp(value, _rotXSliderMin, _rotXSliderMax);
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