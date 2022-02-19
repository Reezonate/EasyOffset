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
            if (_preciseRightResetText.Equals(value)) return;
            _preciseRightResetText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-reset-on-click")]
    [UsedImplicitly]
    private void PreciseRightResetOnClick() {
        if (_preciseRightResetClickedOnce) {
            PluginConfig.CreateUndoStep("Reset Right");
            OnResetButtonPressed(Hand.Right);
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
            if (_preciseRightMirrorText.Equals(value)) return;
            _preciseRightMirrorText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-right-mirror-on-click")]
    [UsedImplicitly]
    private void PreciseRightMirrorOnClick() {
        if (_preciseRightMirrorClickedOnce) {
            PluginConfig.CreateUndoStep("Mirror Right");
            OnMirrorButtonPressed(Hand.Right);
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
            if (_preciseRightZOffset.Equals(value)) return;
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
            if (_preciseRightPosX.Equals(value)) return;
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
            if (_preciseRightPosY.Equals(value)) return;
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
            if (_preciseRightPosZ.Equals(value)) return;
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
            if (_preciseRightRotX.Equals(value)) return;
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
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationX);
        }
    }

    [UIAction("precise-right-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotXIncOnClick() {
        var newValue = StepUp(PreciseRightRotX, _rotSliderIncrement);
        PreciseRightRotX = ClampRot90SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationX);
    }

    [UIAction("precise-right-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotXDecOnClick() {
        var newValue = StepDown(PreciseRightRotX, _rotSliderIncrement);
        PreciseRightRotX = ClampRot90SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
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
            if (_preciseRightRotY.Equals(value)) return;
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
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationY);
        }
    }

    [UIAction("precise-right-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotYIncOnClick() {
        var newValue = StepUp(PreciseRightRotY, _rotSliderIncrement);
        PreciseRightRotY = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationY);
    }

    [UIAction("precise-right-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotYDecOnClick() {
        var newValue = StepDown(PreciseRightRotY, _rotSliderIncrement);
        PreciseRightRotY = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
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
            if (_preciseRightRotZ.Equals(value)) return;
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
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationZ);
        }
    }

    [UIAction("precise-right-rot-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotZIncOnClick() {
        var newValue = StepUp(PreciseRightRotZ, _rotSliderIncrement);
        PreciseRightRotZ = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationZ);
    }

    [UIAction("precise-right-rot-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotZDecOnClick() {
        var newValue = StepDown(PreciseRightRotZ, _rotSliderIncrement);
        PreciseRightRotZ = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
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
            if (_preciseRightRotHor.Equals(value)) return;
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
            RecalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Curve);
        }
    }

    [UIAction("precise-right-rot-hor-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotHorIncOnClick() {
        var newValue = PreciseRightRotHor + _rotSliderIncrement;
        PreciseRightRotHor = ClampRot180SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Curve);
    }

    [UIAction("precise-right-rot-hor-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotHorDecOnClick() {
        var newValue = PreciseRightRotHor - _rotSliderIncrement;
        PreciseRightRotHor = ClampRot180SliderValue(newValue);
        RecalculateControllerSpaceRotations();
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
            if (_preciseRightRotVert.Equals(value)) return;
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
            RecalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
        }
    }

    [UIAction("precise-right-rot-vert-inc-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotVertIncOnClick() {
        var newValue = PreciseRightRotVert + _rotSliderIncrement;
        PreciseRightRotVert = ClampRot90SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
    }

    [UIAction("precise-right-rot-vert-dec-on-click")]
    [UsedImplicitly]
    private void PreciseRightRotVertDecOnClick() {
        var newValue = PreciseRightRotVert - _rotSliderIncrement;
        PreciseRightRotVert = ClampRot90SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
    }

    #endregion

    #region Interactable

    private bool _preciseRightRotationReferenceInteractable = PluginConfig.RightSaberHasReference;

    [UIValue("precise-right-rotation-reference-interactable")]
    [UsedImplicitly]
    private bool PreciseRightRotationReferenceInteractable {
        get => _preciseRightRotationReferenceInteractable;
        set {
            if (_preciseRightRotationReferenceInteractable.Equals(value)) return;
            _preciseRightRotationReferenceInteractable = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region UpdateReference button

    [UIAction("precise-right-update-reference-on-click")]
    [UsedImplicitly]
    private void PreciseRightUpdateReferenceOnClick() {
        PluginConfig.CreateUndoStep("Update right reference");
        PluginConfig.AlignRightReferenceToCurrent();
    }

    #endregion

    #region ClearReference button

    [UIAction("precise-right-clear-reference-on-click")]
    [UsedImplicitly]
    private void PreciseRightClearReferenceOnClick() {
        PluginConfig.CreateUndoStep("Clear right reference");
        PluginConfig.ResetRightSaberReference();
    }

    #endregion
}