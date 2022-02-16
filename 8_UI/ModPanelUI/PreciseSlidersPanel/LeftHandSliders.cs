using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
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
            PluginConfig.ResetOffsets(Hand.Left);
            ResetLeftResetButton();
        } else {
            this.ReInvokeWithDelay(ref _leftResetButtonResetCoroutine, ResetLeftResetButton, ButtonPromptDelaySeconds);
            PreciseLeftResetText = ResetButtonPromptText;
            _preciseLeftResetClickedOnce = true;
        }
    }

    private Coroutine _leftResetButtonResetCoroutine;

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
            this.ReInvokeWithDelay(ref _leftMirrorButtonResetCoroutine, ResetLeftMirrorButton, ButtonPromptDelaySeconds);
            PreciseLeftMirrorText = LeftMirrorButtonPromptText;
            _preciseLeftMirrorClickedOnce = true;
        }
    }

    private Coroutine _leftMirrorButtonResetCoroutine;

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
            OnSmoothValueChanged(null, SmoothValueType.Position);
        } else {
            _preciseLeftZOffset = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-z-offset-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftZOffsetIncOnClick() {
        var newValue = StepUp(PreciseLeftZOffset, _posSliderIncrement);
        PreciseLeftZOffset = ClampZOffsetSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-z-offset-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftZOffsetDecOnClick() {
        var newValue = StepDown(PreciseLeftZOffset, _posSliderIncrement);
        PreciseLeftZOffset = ClampZOffsetSliderValue(newValue);
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
            OnSmoothValueChanged(Hand.Left, SmoothValueType.Position);
        } else {
            _preciseLeftPosX = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosXIncOnClick() {
        var newValue = StepUp(PreciseLeftPosX, _posSliderIncrement);
        PreciseLeftPosX = ClampPosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosXDecOnClick() {
        var newValue = StepDown(PreciseLeftPosX, _posSliderIncrement);
        PreciseLeftPosX = ClampPosSliderValue(newValue);
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
            OnSmoothValueChanged(Hand.Left, SmoothValueType.Position);
        } else {
            _preciseLeftPosY = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosYIncOnClick() {
        var newValue = StepUp(PreciseLeftPosY, _posSliderIncrement);
        PreciseLeftPosY = ClampPosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosYDecOnClick() {
        var newValue = StepDown(PreciseLeftPosY, _posSliderIncrement);
        PreciseLeftPosY = ClampPosSliderValue(newValue);
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
            OnSmoothValueChanged(Hand.Left, SmoothValueType.Position);
        } else {
            _preciseLeftPosZ = value;
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosZIncOnClick() {
        var newValue = StepUp(PreciseLeftPosZ, _posSliderIncrement);
        PreciseLeftPosZ = ClampPosSliderValue(newValue);
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosZDecOnClick() {
        var newValue = StepDown(PreciseLeftPosZ, _posSliderIncrement);
        PreciseLeftPosZ = ClampPosSliderValue(newValue);
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
            OnSmoothValueChanged(Hand.Left, SmoothValueType.Rotation);
        } else {
            _preciseLeftRotX = value;
            CalculateReferenceSpaceRotations();
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotXIncOnClick() {
        var newValue = StepUp(PreciseLeftRotX, _rotSliderIncrement);
        PreciseLeftRotX = ClampRot90SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotXDecOnClick() {
        var newValue = StepDown(PreciseLeftRotX, _rotSliderIncrement);
        PreciseLeftRotX = ClampRot90SliderValue(newValue);
        CalculateReferenceSpaceRotations();
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
            OnSmoothValueChanged(Hand.Left, SmoothValueType.Rotation);
        } else {
            _preciseLeftRotY = value;
            CalculateReferenceSpaceRotations();
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotYIncOnClick() {
        var newValue = StepUp(PreciseLeftRotY, _rotSliderIncrement);
        PreciseLeftRotY = ClampRot180SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotYDecOnClick() {
        var newValue = StepDown(PreciseLeftRotY, _rotSliderIncrement);
        PreciseLeftRotY = ClampRot180SliderValue(newValue);
        CalculateReferenceSpaceRotations();
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
            OnSmoothValueChanged(Hand.Left, SmoothValueType.Rotation);
        } else {
            _preciseLeftRotZ = value;
            CalculateReferenceSpaceRotations();
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-rot-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotZIncOnClick() {
        var newValue = StepUp(PreciseLeftRotZ, _rotSliderIncrement);
        PreciseLeftRotZ = ClampRot180SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotZDecOnClick() {
        var newValue = StepDown(PreciseLeftRotZ, _rotSliderIncrement);
        PreciseLeftRotZ = ClampRot180SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        ApplyPreciseConfig();
    }

    #endregion

    #region RotationHor

    private float _preciseLeftRotHor;

    [UIValue("precise-left-rot-hor-value")]
    [UsedImplicitly]
    private float PreciseLeftRotHor {
        get => _preciseLeftRotHor;
        set {
            _preciseLeftRotHor = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-hor-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotHorOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftRotHorTarget = value;
            OnSmoothValueChanged(Hand.Left, SmoothValueType.RotationReference);
        } else {
            _preciseLeftRotHor = value;
            CalculateControllerSpaceRotations();
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-rot-hor-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotHorIncOnClick() {
        var newValue = StepUp(PreciseLeftRotHor, _rotSliderIncrement);
        PreciseLeftRotHor = ClampRot180SliderValue(newValue);
        CalculateControllerSpaceRotations();
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-hor-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotHorDecOnClick() {
        var newValue = StepDown(PreciseLeftRotHor, _rotSliderIncrement);
        PreciseLeftRotHor = ClampRot180SliderValue(newValue);
        CalculateControllerSpaceRotations();
        ApplyPreciseConfig();
    }

    #endregion

    #region RotationVert

    private float _preciseLeftRotVert;

    [UIValue("precise-left-rot-vert-value")]
    [UsedImplicitly]
    private float PreciseLeftRotVert {
        get => _preciseLeftRotVert;
        set {
            _preciseLeftRotVert = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-vert-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotVertOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftRotVertTarget = value;
            OnSmoothValueChanged(Hand.Left, SmoothValueType.RotationReference);
        } else {
            _preciseLeftRotVert = value;
            CalculateControllerSpaceRotations();
            ApplyPreciseConfig();
        }
    }

    [UIAction("precise-left-rot-vert-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotVertIncOnClick() {
        var newValue = StepUp(PreciseLeftRotVert, _rotSliderIncrement);
        PreciseLeftRotVert = ClampRot90SliderValue(newValue);
        CalculateControllerSpaceRotations();
        ApplyPreciseConfig();
    }

    [UIAction("precise-left-rot-vert-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotVertDecOnClick() {
        var newValue = StepDown(PreciseLeftRotVert, _rotSliderIncrement);
        PreciseLeftRotVert = ClampRot90SliderValue(newValue);
        CalculateControllerSpaceRotations();
        ApplyPreciseConfig();
    }

    #endregion
}