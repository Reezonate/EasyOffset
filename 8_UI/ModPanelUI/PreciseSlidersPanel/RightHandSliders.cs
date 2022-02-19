using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
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
            PluginConfig.CreateUndoStep("Reset Right");
            PreciseReset(Hand.Right);
            ResetRightResetButton();
        } else {
            this.ReInvokeWithDelay(ref _rightResetButtonResetCoroutine, ResetRightResetButton, ButtonPromptDelaySeconds);
            PreciseRightResetText = ButtonPromptText;
            _preciseRightResetClickedOnce = true;
        }
    }

    private Coroutine _rightResetButtonResetCoroutine;

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
            PluginConfig.CreateUndoStep("Mirror Right");
            PreciseMirror(Hand.Right);
            ResetRightMirrorButton();
        } else {
            this.ReInvokeWithDelay(ref _rightMirrorButtonResetCoroutine, ResetRightMirrorButton, ButtonPromptDelaySeconds);
            PreciseRightMirrorText = ButtonPromptText;
            _preciseRightMirrorClickedOnce = true;
        }
    }

    private Coroutine _rightMirrorButtonResetCoroutine;

    private void ResetRightMirrorButton() {
        PreciseRightMirrorText = RightMirrorButtonIdleText;
        _preciseRightMirrorClickedOnce = false;
    }

    #endregion

    #region Combined Position

    private Vector3 PreciseRightPivotPosition {
        get => new(PreciseRightPosX, PreciseRightPosY, PreciseRightPosZ);
        set {
            PreciseRightPosX = value.x;
            PreciseRightPosY = value.y;
            PreciseRightPosZ = value.z;
        }
    }

    #endregion

    #region Combined Rotation

    private Quaternion PreciseRightRotation => TransformUtils.RotationFromEuler(PreciseRightRotationEuler);

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
            OnSliderTargetChanged(Hand.Right, SliderValueType.ZOffset);
        } else {
            _preciseRightZOffset = value;
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.ZOffset);
        }
    }

    [UIAction("precise-right-z-offset-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightZOffsetIncOnClick() {
        var newValue = StepUp(PreciseRightZOffset, _posSliderIncrement);
        PreciseRightZOffset = ClampZOffsetSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.ZOffset);
    }

    [UIAction("precise-right-z-offset-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightZOffsetDecOnClick() {
        var newValue = StepDown(PreciseRightZOffset, _posSliderIncrement);
        PreciseRightZOffset = ClampZOffsetSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.ZOffset);
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
            OnSliderTargetChanged(Hand.Right, SliderValueType.PositionX);
        } else {
            _preciseRightPosX = value;
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionX);
        }
    }

    [UIAction("precise-right-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosXIncOnClick() {
        var newValue = StepUp(PreciseRightPosX, _posSliderIncrement);
        PreciseRightPosX = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionX);
    }

    [UIAction("precise-right-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosXDecOnClick() {
        var newValue = StepDown(PreciseRightPosX, _posSliderIncrement);
        PreciseRightPosX = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionX);
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
            OnSliderTargetChanged(Hand.Right, SliderValueType.PositionY);
        } else {
            _preciseRightPosY = value;
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionY);
        }
    }

    [UIAction("precise-right-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosYIncOnClick() {
        var newValue = StepUp(PreciseRightPosY, _posSliderIncrement);
        PreciseRightPosY = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionY);
    }

    [UIAction("precise-right-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosYDecOnClick() {
        var newValue = StepDown(PreciseRightPosY, _posSliderIncrement);
        PreciseRightPosY = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionY);
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
            OnSliderTargetChanged(Hand.Right, SliderValueType.PositionZ);
        } else {
            _preciseRightPosZ = value;
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionZ);
        }
    }

    [UIAction("precise-right-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosZIncOnClick() {
        var newValue = StepUp(PreciseRightPosZ, _posSliderIncrement);
        PreciseRightPosZ = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionZ);
    }

    [UIAction("precise-right-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightPosZDecOnClick() {
        var newValue = StepDown(PreciseRightPosZ, _posSliderIncrement);
        PreciseRightPosZ = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionZ);
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
            OnSliderTargetChanged(Hand.Right, SliderValueType.RotationX);
        } else {
            _preciseRightRotX = value;
            CalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationX);
        }
    }

    [UIAction("precise-right-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotXIncOnClick() {
        var newValue = StepUp(PreciseRightRotX, _rotSliderIncrement);
        PreciseRightRotX = ClampRot90SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationX);
    }

    [UIAction("precise-right-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotXDecOnClick() {
        var newValue = StepDown(PreciseRightRotX, _rotSliderIncrement);
        PreciseRightRotX = ClampRot90SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationX);
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
            OnSliderTargetChanged(Hand.Right, SliderValueType.RotationY);
        } else {
            _preciseRightRotY = value;
            CalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationY);
        }
    }

    [UIAction("precise-right-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotYIncOnClick() {
        var newValue = StepUp(PreciseRightRotY, _rotSliderIncrement);
        PreciseRightRotY = ClampRot180SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationY);
    }

    [UIAction("precise-right-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotYDecOnClick() {
        var newValue = StepDown(PreciseRightRotY, _rotSliderIncrement);
        PreciseRightRotY = ClampRot180SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationY);
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
            OnSliderTargetChanged(Hand.Right, SliderValueType.RotationZ);
        } else {
            _preciseRightRotZ = value;
            CalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationZ);
        }
    }

    [UIAction("precise-right-rot-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotZIncOnClick() {
        var newValue = StepUp(PreciseRightRotZ, _rotSliderIncrement);
        PreciseRightRotZ = ClampRot180SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationZ);
    }

    [UIAction("precise-right-rot-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotZDecOnClick() {
        var newValue = StepDown(PreciseRightRotZ, _rotSliderIncrement);
        PreciseRightRotZ = ClampRot180SliderValue(newValue);
        CalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationZ);
    }

    #endregion

    #region Curve

    private float _preciseRightRotHor;

    [UIValue("precise-right-rot-hor-value")]
    [UsedImplicitly]
    private float PreciseRightRotHor {
        get => _preciseRightRotHor;
        set {
            _preciseRightRotHor = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-rot-hor-on-change")]
    [UsedImplicitly]
    private void PreciseRightRotHorOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseRightRotHorTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.Curve);
        } else {
            _preciseRightRotHor = value;
            CalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Curve);
        }
    }

    [UIAction("precise-right-rot-hor-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotHorIncOnClick() {
        var newValue = PreciseRightRotHor + _rotSliderIncrement;
        PreciseRightRotHor = ClampRot180SliderValue(newValue);
        CalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Curve);
    }

    [UIAction("precise-right-rot-hor-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotHorDecOnClick() {
        var newValue = PreciseRightRotHor - _rotSliderIncrement;
        PreciseRightRotHor = ClampRot180SliderValue(newValue);
        CalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Curve);
    }

    #endregion

    #region Balance

    private float _preciseRightRotVert;

    [UIValue("precise-right-rot-vert-value")]
    [UsedImplicitly]
    private float PreciseRightRotVert {
        get => _preciseRightRotVert;
        set {
            _preciseRightRotVert = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-rot-vert-on-change")]
    [UsedImplicitly]
    private void PreciseRightRotVertOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseRightRotVertTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.Balance);
        } else {
            _preciseRightRotVert = value;
            CalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
        }
    }

    [UIAction("precise-right-rot-vert-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotVertIncOnClick() {
        var newValue = PreciseRightRotVert + _rotSliderIncrement;
        PreciseRightRotVert = ClampRot90SliderValue(newValue);
        CalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
    }

    [UIAction("precise-right-rot-vert-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotVertDecOnClick() {
        var newValue = PreciseRightRotVert - _rotSliderIncrement;
        PreciseRightRotVert = ClampRot90SliderValue(newValue);
        CalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
    }

    #endregion
}