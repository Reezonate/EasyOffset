using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class SlidersPanel {
    #region Reset Button

    private string _directRightResetText = ResetButtonIdleText;
    private bool _directRightResetClickedOnce;

    [UIValue("direct-right-reset-text")]
    [UsedImplicitly]
    private string DirectRightResetText {
        get => _directRightResetText;
        set {
            if (_directRightResetText.Equals(value)) return;
            _directRightResetText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-reset-on-click")]
    [UsedImplicitly]
    private void DirectRightResetOnClick() {
        if (_directRightResetClickedOnce) {
            PluginConfig.CreateUndoStep("Reset Right");
            OnResetButtonPressed(Hand.Right);
            ResetRightResetButton();
        } else {
            this.ReInvokeWithDelay(ref _rightResetButtonResetCoroutine, ResetRightResetButton, ButtonPromptDelaySeconds);
            DirectRightResetText = ButtonPromptText;
            _directRightResetClickedOnce = true;
        }
    }

    private Coroutine _rightResetButtonResetCoroutine;

    private void ResetRightResetButton() {
        DirectRightResetText = ResetButtonIdleText;
        _directRightResetClickedOnce = false;
    }

    #endregion

    #region MirrorButton

    private string _directRightMirrorText = RightMirrorButtonIdleText;
    private bool _directRightMirrorClickedOnce;

    [UIValue("direct-right-mirror-text")]
    [UsedImplicitly]
    private string DirectRightMirrorText {
        get => _directRightMirrorText;
        set {
            if (_directRightMirrorText.Equals(value)) return;
            _directRightMirrorText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-mirror-on-click")]
    [UsedImplicitly]
    private void DirectRightMirrorOnClick() {
        if (_directRightMirrorClickedOnce) {
            PluginConfig.CreateUndoStep("Mirror to Left");
            OnMirrorButtonPressed(Hand.Right);
            ResetRightMirrorButton();
        } else {
            this.ReInvokeWithDelay(ref _rightMirrorButtonResetCoroutine, ResetRightMirrorButton, ButtonPromptDelaySeconds);
            DirectRightMirrorText = ButtonPromptText;
            _directRightMirrorClickedOnce = true;
        }
    }

    private Coroutine _rightMirrorButtonResetCoroutine;

    private void ResetRightMirrorButton() {
        DirectRightMirrorText = RightMirrorButtonIdleText;
        _directRightMirrorClickedOnce = false;
    }

    #endregion

    #region Combined Position

    private Vector3 DirectRightPivotPosition {
        get => new(DirectRightPosX, DirectRightPosY, DirectRightPosZ);
        set {
            DirectRightPosX = value.x;
            DirectRightPosY = value.y;
            DirectRightPosZ = value.z;
        }
    }

    #endregion

    #region Combined Rotation

    private Quaternion DirectRightRotation => TransformUtils.RotationFromEuler(DirectRightRotationEuler);

    private Vector3 DirectRightRotationEuler {
        get => new(DirectRightRotX, DirectRightRotY, DirectRightRotZ);
        set {
            DirectRightRotX = value.x;
            DirectRightRotY = value.y;
            DirectRightRotZ = value.z;
        }
    }

    #endregion

    #region ZOffset

    private float _directRightZOffset;

    [UIValue("direct-right-z-offset-value")]
    [UsedImplicitly]
    private float DirectRightZOffset {
        get => _directRightZOffset;
        set {
            if (_directRightZOffset.Equals(value)) return;
            _directRightZOffset = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-z-offset-on-change")]
    [UsedImplicitly]
    private void DirectRightZOffsetOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightZOffsetTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.ZOffset);
        } else {
            _directRightZOffset = value;
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.ZOffset);
        }
    }

    [UIAction("direct-right-z-offset-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightZOffsetIncOnClick() {
        var newValue = StepUp(DirectRightZOffset, _posSliderIncrement);
        DirectRightZOffset = ClampZOffsetSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.ZOffset);
    }

    [UIAction("direct-right-z-offset-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightZOffsetDecOnClick() {
        var newValue = StepDown(DirectRightZOffset, _posSliderIncrement);
        DirectRightZOffset = ClampZOffsetSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.ZOffset);
    }

    #endregion

    #region PositionX

    private float _directRightPosX;

    [UIValue("direct-right-pos-x-value")]
    [UsedImplicitly]
    private float DirectRightPosX {
        get => _directRightPosX;
        set {
            if (_directRightPosX.Equals(value)) return;
            _directRightPosX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-pos-x-on-change")]
    [UsedImplicitly]
    private void DirectRightPosXOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightPosXTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.PositionX);
        } else {
            _directRightPosX = value;
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionX);
        }
    }

    [UIAction("direct-right-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightPosXIncOnClick() {
        var newValue = StepUp(DirectRightPosX, _posSliderIncrement);
        DirectRightPosX = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionX);
    }

    [UIAction("direct-right-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightPosXDecOnClick() {
        var newValue = StepDown(DirectRightPosX, _posSliderIncrement);
        DirectRightPosX = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionX);
    }

    #endregion

    #region PositionY

    private float _directRightPosY;

    [UIValue("direct-right-pos-y-value")]
    [UsedImplicitly]
    private float DirectRightPosY {
        get => _directRightPosY;
        set {
            if (_directRightPosY.Equals(value)) return;
            _directRightPosY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-pos-y-on-change")]
    [UsedImplicitly]
    private void DirectRightPosYOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightPosYTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.PositionY);
        } else {
            _directRightPosY = value;
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionY);
        }
    }

    [UIAction("direct-right-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightPosYIncOnClick() {
        var newValue = StepUp(DirectRightPosY, _posSliderIncrement);
        DirectRightPosY = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionY);
    }

    [UIAction("direct-right-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightPosYDecOnClick() {
        var newValue = StepDown(DirectRightPosY, _posSliderIncrement);
        DirectRightPosY = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionY);
    }

    #endregion

    #region PositionZ

    private float _directRightPosZ;

    [UIValue("direct-right-pos-z-value")]
    [UsedImplicitly]
    private float DirectRightPosZ {
        get => _directRightPosZ;
        set {
            if (_directRightPosZ.Equals(value)) return;
            _directRightPosZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-pos-z-on-change")]
    [UsedImplicitly]
    private void DirectRightPosZOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightPosZTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.PositionZ);
        } else {
            _directRightPosZ = value;
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionZ);
        }
    }

    [UIAction("direct-right-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightPosZIncOnClick() {
        var newValue = StepUp(DirectRightPosZ, _posSliderIncrement);
        DirectRightPosZ = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionZ);
    }

    [UIAction("direct-right-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightPosZDecOnClick() {
        var newValue = StepDown(DirectRightPosZ, _posSliderIncrement);
        DirectRightPosZ = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.PositionZ);
    }

    #endregion

    #region RotationX

    private float _directRightRotX;

    [UIValue("direct-right-rot-x-value")]
    [UsedImplicitly]
    private float DirectRightRotX {
        get => _directRightRotX;
        set {
            if (_directRightRotX.Equals(value)) return;
            _directRightRotX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-rot-x-on-change")]
    [UsedImplicitly]
    private void DirectRightRotXOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightRotXTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.RotationX);
        } else {
            _directRightRotX = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationX);
        }
    }

    [UIAction("direct-right-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightRotXIncOnClick() {
        var newValue = StepUp(DirectRightRotX, _rotSliderIncrement);
        DirectRightRotX = ClampRot90SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationX);
    }

    [UIAction("direct-right-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightRotXDecOnClick() {
        var newValue = StepDown(DirectRightRotX, _rotSliderIncrement);
        DirectRightRotX = ClampRot90SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationX);
    }

    #endregion

    #region RotationY

    private float _directRightRotY;

    [UIValue("direct-right-rot-y-value")]
    [UsedImplicitly]
    private float DirectRightRotY {
        get => _directRightRotY;
        set {
            if (_directRightRotY.Equals(value)) return;
            _directRightRotY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-rot-y-on-change")]
    [UsedImplicitly]
    private void DirectRightRotYOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightRotYTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.RotationY);
        } else {
            _directRightRotY = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationY);
        }
    }

    [UIAction("direct-right-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightRotYIncOnClick() {
        var newValue = StepUp(DirectRightRotY, _rotSliderIncrement);
        DirectRightRotY = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationY);
    }

    [UIAction("direct-right-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightRotYDecOnClick() {
        var newValue = StepDown(DirectRightRotY, _rotSliderIncrement);
        DirectRightRotY = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationY);
    }

    #endregion

    #region RotationZ

    private float _directRightRotZ;

    [UIValue("direct-right-rot-z-value")]
    [UsedImplicitly]
    private float DirectRightRotZ {
        get => _directRightRotZ;
        set {
            if (_directRightRotZ.Equals(value)) return;
            _directRightRotZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-rot-z-on-change")]
    [UsedImplicitly]
    private void DirectRightRotZOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightRotZTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.RotationZ);
        } else {
            _directRightRotZ = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationZ);
        }
    }

    [UIAction("direct-right-rot-z-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightRotZIncOnClick() {
        var newValue = StepUp(DirectRightRotZ, _rotSliderIncrement);
        DirectRightRotZ = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationZ);
    }

    [UIAction("direct-right-rot-z-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightRotZDecOnClick() {
        var newValue = StepDown(DirectRightRotZ, _rotSliderIncrement);
        DirectRightRotZ = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.RotationZ);
    }

    #endregion

    #region Curve

    private float _directRightRotHor;

    [UIValue("direct-right-rot-hor-value")]
    [UsedImplicitly]
    private float DirectRightRotHor {
        get => _directRightRotHor;
        set {
            if (_directRightRotHor.Equals(value)) return;
            _directRightRotHor = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-rot-hor-on-change")]
    [UsedImplicitly]
    private void DirectRightRotHorOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightRotHorTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.Curve);
        } else {
            _directRightRotHor = value;
            RecalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Curve);
        }
    }

    [UIAction("direct-right-rot-hor-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightRotHorIncOnClick() {
        var newValue = DirectRightRotHor + _rotSliderIncrement;
        DirectRightRotHor = ClampRot180SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Curve);
    }

    [UIAction("direct-right-rot-hor-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightRotHorDecOnClick() {
        var newValue = DirectRightRotHor - _rotSliderIncrement;
        DirectRightRotHor = ClampRot180SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Curve);
    }

    #endregion

    #region Balance

    private float _directRightRotVert;

    [UIValue("direct-right-rot-vert-value")]
    [UsedImplicitly]
    private float DirectRightRotVert {
        get => _directRightRotVert;
        set {
            if (_directRightRotVert.Equals(value)) return;
            _directRightRotVert = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-right-rot-vert-on-change")]
    [UsedImplicitly]
    private void DirectRightRotVertOnChange(float value) {
        if (_smoothingEnabled) {
            _directRightRotVertTarget = value;
            OnSliderTargetChanged(Hand.Right, SliderValueType.Balance);
        } else {
            _directRightRotVert = value;
            RecalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
        }
    }

    [UIAction("direct-right-rot-vert-inc-on-click")]
    [UsedImplicitly]
    private void DirectRightRotVertIncOnClick() {
        var newValue = DirectRightRotVert + _rotSliderIncrement;
        DirectRightRotVert = ClampRot90SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
    }

    [UIAction("direct-right-rot-vert-dec-on-click")]
    [UsedImplicitly]
    private void DirectRightRotVertDecOnClick() {
        var newValue = DirectRightRotVert - _rotSliderIncrement;
        DirectRightRotVert = ClampRot90SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Right, SliderValueType.Balance);
    }

    #endregion

    #region Interactable

    private bool _directRightRotationReferenceInteractable = PluginConfig.RightSaberHasReference;

    [UIValue("direct-right-rotation-reference-interactable")]
    [UsedImplicitly]
    private bool DirectRightRotationReferenceInteractable {
        get => _directRightRotationReferenceInteractable;
        set {
            if (_directRightRotationReferenceInteractable.Equals(value)) return;
            _directRightRotationReferenceInteractable = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Help Active

    private bool _directRightHelpActive;

    [UIValue("direct-right-help-active")]
    [UsedImplicitly]
    private bool DirectRightHelpActive {
        get => _directRightHelpActive;
        set {
            if (_directRightHelpActive.Equals(value)) return;
            _directRightHelpActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Buttons Active

    private bool _directRightButtonsActive;

    [UIValue("direct-right-buttons-active")]
    [UsedImplicitly]
    private bool DirectRightButtonsActive {
        get => _directRightButtonsActive;
        set {
            if (_directRightButtonsActive.Equals(value)) return;
            _directRightButtonsActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region UpdateReference button

    [UIAction("direct-right-update-reference-on-click")]
    [UsedImplicitly]
    private void DirectRightUpdateReferenceOnClick() {
        PluginConfig.CreateUndoStep("Update right reference");
        PluginConfig.AlignRightReferenceToCurrent();
    }

    #endregion

    #region ClearReference button

    [UIAction("direct-right-clear-reference-on-click")]
    [UsedImplicitly]
    private void DirectRightClearReferenceOnClick() {
        PluginConfig.CreateUndoStep("Clear right reference");
        PluginConfig.ResetRightSaberReference();
    }

    #endregion
}