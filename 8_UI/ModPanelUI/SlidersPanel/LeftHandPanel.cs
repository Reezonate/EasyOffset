using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class SlidersPanel {
    #region Reset Button

    private string _directLeftResetText = ResetButtonIdleText;
    private bool _directLeftResetClickedOnce;

    [UIValue("direct-left-reset-text")]
    [UsedImplicitly]
    private string DirectLeftResetText {
        get => _directLeftResetText;
        set {
            if (_directLeftResetText.Equals(value)) return;
            _directLeftResetText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-reset-on-click")]
    [UsedImplicitly]
    private void DirectLeftResetOnClick() {
        if (_directLeftResetClickedOnce) {
            PluginConfig.CreateUndoStep("Reset Left");
            OnResetButtonPressed(Hand.Left);
            ResetLeftResetButton();
        } else {
            this.ReInvokeWithDelay(ref _leftResetButtonResetCoroutine, ResetLeftResetButton, ButtonPromptDelaySeconds);
            DirectLeftResetText = ButtonPromptText;
            _directLeftResetClickedOnce = true;
        }
    }

    private Coroutine _leftResetButtonResetCoroutine;

    private void ResetLeftResetButton() {
        DirectLeftResetText = ResetButtonIdleText;
        _directLeftResetClickedOnce = false;
    }

    #endregion

    #region MirrorButton

    private string _directLeftMirrorText = LeftMirrorButtonIdleText;
    private bool _directLeftMirrorClickedOnce;

    [UIValue("direct-left-mirror-text")]
    [UsedImplicitly]
    private string DirectLeftMirrorText {
        get => _directLeftMirrorText;
        set {
            if (_directLeftMirrorText.Equals(value)) return;
            _directLeftMirrorText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-mirror-on-click")]
    [UsedImplicitly]
    private void DirectLeftMirrorOnClick() {
        if (_directLeftMirrorClickedOnce) {
            PluginConfig.CreateUndoStep("Mirror to Right");
            OnMirrorButtonPressed(Hand.Left);
            ResetLeftMirrorButton();
        } else {
            this.ReInvokeWithDelay(ref _leftMirrorButtonResetCoroutine, ResetLeftMirrorButton, ButtonPromptDelaySeconds);
            DirectLeftMirrorText = ButtonPromptText;
            _directLeftMirrorClickedOnce = true;
        }
    }

    private Coroutine _leftMirrorButtonResetCoroutine;

    private void ResetLeftMirrorButton() {
        DirectLeftMirrorText = LeftMirrorButtonIdleText;
        _directLeftMirrorClickedOnce = false;
    }

    #endregion

    #region Combined Position

    private Vector3 DirectLeftPivotPosition {
        get => new(DirectLeftPosX, DirectLeftPosY, DirectLeftPosZ);
        set {
            DirectLeftPosX = value.x;
            DirectLeftPosY = value.y;
            DirectLeftPosZ = value.z;
        }
    }

    #endregion

    #region Combined Rotation

    private Quaternion DirectLeftRotation => TransformUtils.RotationFromEuler(DirectLeftRotationEuler);

    private Vector3 DirectLeftRotationEuler {
        get => new(DirectLeftRotX, DirectLeftRotY, DirectLeftRotZ);
        set {
            DirectLeftRotX = value.x;
            DirectLeftRotY = value.y;
            DirectLeftRotZ = value.z;
        }
    }

    #endregion

    #region ZOffset

    private float _directLeftZOffset;

    [UIValue("direct-left-z-offset-value")]
    [UsedImplicitly]
    private float DirectLeftZOffset {
        get => _directLeftZOffset;
        set {
            if (_directLeftZOffset.Equals(value)) return;
            _directLeftZOffset = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-z-offset-on-change")]
    [UsedImplicitly]
    private void DirectLeftZOffsetOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftZOffsetTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.ZOffset);
        } else {
            _directLeftZOffset = value;
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.ZOffset);
        }
    }

    [UIAction("direct-left-z-offset-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftZOffsetIncOnClick() {
        var newValue = StepUp(DirectLeftZOffset, _posSliderIncrement);
        DirectLeftZOffset = ClampZOffsetSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.ZOffset);
    }

    [UIAction("direct-left-z-offset-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftZOffsetDecOnClick() {
        var newValue = StepDown(DirectLeftZOffset, _posSliderIncrement);
        DirectLeftZOffset = ClampZOffsetSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.ZOffset);
    }

    #endregion

    #region PositionX

    private float _directLeftPosX;

    [UIValue("direct-left-pos-x-value")]
    [UsedImplicitly]
    private float DirectLeftPosX {
        get => _directLeftPosX;
        set {
            if (_directLeftPosX.Equals(value)) return;
            _directLeftPosX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-pos-x-on-change")]
    [UsedImplicitly]
    private void DirectLeftPosXOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftPosXTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.PositionX);
        } else {
            _directLeftPosX = value;
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionX);
        }
    }

    [UIAction("direct-left-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftPosXIncOnClick() {
        var newValue = StepUp(DirectLeftPosX, _posSliderIncrement);
        DirectLeftPosX = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionX);
    }

    [UIAction("direct-left-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftPosXDecOnClick() {
        var newValue = StepDown(DirectLeftPosX, _posSliderIncrement);
        DirectLeftPosX = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionX);
    }

    #endregion

    #region PositionY

    private float _directLeftPosY;

    [UIValue("direct-left-pos-y-value")]
    [UsedImplicitly]
    private float DirectLeftPosY {
        get => _directLeftPosY;
        set {
            if (_directLeftPosY.Equals(value)) return;
            _directLeftPosY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-pos-y-on-change")]
    [UsedImplicitly]
    private void DirectLeftPosYOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftPosYTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.PositionY);
        } else {
            _directLeftPosY = value;
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionY);
        }
    }

    [UIAction("direct-left-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftPosYIncOnClick() {
        var newValue = StepUp(DirectLeftPosY, _posSliderIncrement);
        DirectLeftPosY = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionY);
    }

    [UIAction("direct-left-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftPosYDecOnClick() {
        var newValue = StepDown(DirectLeftPosY, _posSliderIncrement);
        DirectLeftPosY = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionY);
    }

    #endregion

    #region PositionZ

    private float _directLeftPosZ;

    [UIValue("direct-left-pos-z-value")]
    [UsedImplicitly]
    private float DirectLeftPosZ {
        get => _directLeftPosZ;
        set {
            if (_directLeftPosZ.Equals(value)) return;
            _directLeftPosZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-pos-z-on-change")]
    [UsedImplicitly]
    private void DirectLeftPosZOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftPosZTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.PositionZ);
        } else {
            _directLeftPosZ = value;
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionZ);
        }
    }

    [UIAction("direct-left-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftPosZIncOnClick() {
        var newValue = StepUp(DirectLeftPosZ, _posSliderIncrement);
        DirectLeftPosZ = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionZ);
    }

    [UIAction("direct-left-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftPosZDecOnClick() {
        var newValue = StepDown(DirectLeftPosZ, _posSliderIncrement);
        DirectLeftPosZ = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionZ);
    }

    #endregion

    #region RotationX

    private float _directLeftRotX;

    [UIValue("direct-left-rot-x-value")]
    [UsedImplicitly]
    private float DirectLeftRotX {
        get => _directLeftRotX;
        set {
            if (_directLeftRotX.Equals(value)) return;
            _directLeftRotX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-rot-x-on-change")]
    [UsedImplicitly]
    private void DirectLeftRotXOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftRotXTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.RotationX);
        } else {
            _directLeftRotX = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationX);
        }
    }

    [UIAction("direct-left-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotXIncOnClick() {
        var newValue = StepUp(DirectLeftRotX, _rotSliderIncrement);
        DirectLeftRotX = ClampRot90SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationX);
    }

    [UIAction("direct-left-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotXDecOnClick() {
        var newValue = StepDown(DirectLeftRotX, _rotSliderIncrement);
        DirectLeftRotX = ClampRot90SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationX);
    }

    #endregion

    #region RotationY

    private float _directLeftRotY;

    [UIValue("direct-left-rot-y-value")]
    [UsedImplicitly]
    private float DirectLeftRotY {
        get => _directLeftRotY;
        set {
            if (_directLeftRotY.Equals(value)) return;
            _directLeftRotY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-rot-y-on-change")]
    [UsedImplicitly]
    private void DirectLeftRotYOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftRotYTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.RotationY);
        } else {
            _directLeftRotY = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationY);
        }
    }

    [UIAction("direct-left-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotYIncOnClick() {
        var newValue = StepUp(DirectLeftRotY, _rotSliderIncrement);
        DirectLeftRotY = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationY);
    }

    [UIAction("direct-left-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotYDecOnClick() {
        var newValue = StepDown(DirectLeftRotY, _rotSliderIncrement);
        DirectLeftRotY = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationY);
    }

    #endregion

    #region RotationZ

    private float _directLeftRotZ;

    [UIValue("direct-left-rot-z-value")]
    [UsedImplicitly]
    private float DirectLeftRotZ {
        get => _directLeftRotZ;
        set {
            if (_directLeftRotZ.Equals(value)) return;
            _directLeftRotZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-rot-z-on-change")]
    [UsedImplicitly]
    private void DirectLeftRotZOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftRotZTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.RotationZ);
        } else {
            _directLeftRotZ = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationZ);
        }
    }

    [UIAction("direct-left-rot-z-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotZIncOnClick() {
        var newValue = StepUp(DirectLeftRotZ, _rotSliderIncrement);
        DirectLeftRotZ = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationZ);
    }

    [UIAction("direct-left-rot-z-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotZDecOnClick() {
        var newValue = StepDown(DirectLeftRotZ, _rotSliderIncrement);
        DirectLeftRotZ = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationZ);
    }

    #endregion

    #region Curve

    private float _directLeftRotHor;

    [UIValue("direct-left-rot-hor-value")]
    [UsedImplicitly]
    private float DirectLeftRotHor {
        get => _directLeftRotHor;
        set {
            if (_directLeftRotHor.Equals(value)) return;
            _directLeftRotHor = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-rot-hor-on-change")]
    [UsedImplicitly]
    private void DirectLeftRotHorOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftRotHorTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.Curve);
        } else {
            _directLeftRotHor = value;
            RecalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Curve);
        }
    }

    [UIAction("direct-left-rot-hor-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotHorIncOnClick() {
        var newValue = DirectLeftRotHor + _rotSliderIncrement;
        DirectLeftRotHor = ClampRot180SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Curve);
    }

    [UIAction("direct-left-rot-hor-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotHorDecOnClick() {
        var newValue = DirectLeftRotHor - _rotSliderIncrement;
        DirectLeftRotHor = ClampRot180SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Curve);
    }

    #endregion

    #region Balance

    private float _directLeftRotVert;

    [UIValue("direct-left-rot-vert-value")]
    [UsedImplicitly]
    private float DirectLeftRotVert {
        get => _directLeftRotVert;
        set {
            if (_directLeftRotVert.Equals(value)) return;
            _directLeftRotVert = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("direct-left-rot-vert-on-change")]
    [UsedImplicitly]
    private void DirectLeftRotVertOnChange(float value) {
        if (_smoothingEnabled) {
            _directLeftRotVertTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.Balance);
        } else {
            _directLeftRotVert = value;
            RecalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Balance);
        }
    }

    [UIAction("direct-left-rot-vert-inc-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotVertIncOnClick() {
        var newValue = DirectLeftRotVert + _rotSliderIncrement;
        DirectLeftRotVert = ClampRot90SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Balance);
    }

    [UIAction("direct-left-rot-vert-dec-on-click")]
    [UsedImplicitly]
    private void DirectLeftRotVertDecOnClick() {
        var newValue = DirectLeftRotVert - _rotSliderIncrement;
        DirectLeftRotVert = ClampRot90SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Balance);
    }

    #endregion

    #region Interactable

    private bool _directLeftRotationReferenceInteractable = PluginConfig.LeftSaberHasReference;

    [UIValue("direct-left-rotation-reference-interactable")]
    [UsedImplicitly]
    private bool DirectLeftRotationReferenceInteractable {
        get => _directLeftRotationReferenceInteractable;
        set {
            if (_directLeftRotationReferenceInteractable.Equals(value)) return;
            _directLeftRotationReferenceInteractable = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Help Active

    private bool _directLeftHelpActive;

    [UIValue("direct-left-help-active")]
    [UsedImplicitly]
    private bool DirectLeftHelpActive {
        get => _directLeftHelpActive;
        set {
            if (_directLeftHelpActive.Equals(value)) return;
            _directLeftHelpActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Buttons Active

    private bool _directLeftButtonsActive;

    [UIValue("direct-left-buttons-active")]
    [UsedImplicitly]
    private bool DirectLeftButtonsActive {
        get => _directLeftButtonsActive;
        set {
            if (_directLeftButtonsActive.Equals(value)) return;
            _directLeftButtonsActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region UpdateReference button

    [UIAction("direct-left-update-reference-on-click")]
    [UsedImplicitly]
    private void DirectLeftUpdateReferenceOnClick() {
        PluginConfig.CreateUndoStep("Update left reference");
        PluginConfig.AlignLeftReferenceToCurrent();
    }

    #endregion

    #region ClearReference button

    [UIAction("direct-left-clear-reference-on-click")]
    [UsedImplicitly]
    private void DirectLeftClearReferenceOnClick() {
        PluginConfig.CreateUndoStep("Clear left reference");
        PluginConfig.ResetLeftSaberReference();
    }

    #endregion
}