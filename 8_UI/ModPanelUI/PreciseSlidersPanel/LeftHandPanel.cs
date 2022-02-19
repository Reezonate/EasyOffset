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
            if (_preciseLeftResetText.Equals(value)) return;
            _preciseLeftResetText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-reset-on-click")]
    [UsedImplicitly]
    private void PreciseLeftResetOnClick() {
        if (_preciseLeftResetClickedOnce) {
            PluginConfig.CreateUndoStep("Reset Left");
            OnResetButtonPressed(Hand.Left);
            ResetLeftResetButton();
        } else {
            this.ReInvokeWithDelay(ref _leftResetButtonResetCoroutine, ResetLeftResetButton, ButtonPromptDelaySeconds);
            PreciseLeftResetText = ButtonPromptText;
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
            if (_preciseLeftMirrorText.Equals(value)) return;
            _preciseLeftMirrorText = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-mirror-on-click")]
    [UsedImplicitly]
    private void PreciseLeftMirrorOnClick() {
        if (_preciseLeftMirrorClickedOnce) {
            PluginConfig.CreateUndoStep("Mirror Left");
            OnMirrorButtonPressed(Hand.Left);
            ResetLeftMirrorButton();
        } else {
            this.ReInvokeWithDelay(ref _leftMirrorButtonResetCoroutine, ResetLeftMirrorButton, ButtonPromptDelaySeconds);
            PreciseLeftMirrorText = ButtonPromptText;
            _preciseLeftMirrorClickedOnce = true;
        }
    }

    private Coroutine _leftMirrorButtonResetCoroutine;

    private void ResetLeftMirrorButton() {
        PreciseLeftMirrorText = LeftMirrorButtonIdleText;
        _preciseLeftMirrorClickedOnce = false;
    }

    #endregion

    #region Combined Position

    private Vector3 PreciseLeftPivotPosition {
        get => new(PreciseLeftPosX, PreciseLeftPosY, PreciseLeftPosZ);
        set {
            PreciseLeftPosX = value.x;
            PreciseLeftPosY = value.y;
            PreciseLeftPosZ = value.z;
        }
    }

    #endregion

    #region Combined Rotation

    private Quaternion PreciseLeftRotation => TransformUtils.RotationFromEuler(PreciseLeftRotationEuler);

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
            if (_preciseLeftZOffset.Equals(value)) return;
            _preciseLeftZOffset = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-z-offset-on-change")]
    [UsedImplicitly]
    private void PreciseLeftZOffsetOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftZOffsetTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.ZOffset);
        } else {
            _preciseLeftZOffset = value;
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.ZOffset);
        }
    }

    [UIAction("precise-left-z-offset-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftZOffsetIncOnClick() {
        var newValue = StepUp(PreciseLeftZOffset, _posSliderIncrement);
        PreciseLeftZOffset = ClampZOffsetSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.ZOffset);
    }

    [UIAction("precise-left-z-offset-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftZOffsetDecOnClick() {
        var newValue = StepDown(PreciseLeftZOffset, _posSliderIncrement);
        PreciseLeftZOffset = ClampZOffsetSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.ZOffset);
    }

    #endregion

    #region PositionX

    private float _preciseLeftPosX;

    [UIValue("precise-left-pos-x-value")]
    [UsedImplicitly]
    private float PreciseLeftPosX {
        get => _preciseLeftPosX;
        set {
            if (_preciseLeftPosX.Equals(value)) return;
            _preciseLeftPosX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-pos-x-on-change")]
    [UsedImplicitly]
    private void PreciseLeftPosXOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftPosXTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.PositionX);
        } else {
            _preciseLeftPosX = value;
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionX);
        }
    }

    [UIAction("precise-left-pos-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosXIncOnClick() {
        var newValue = StepUp(PreciseLeftPosX, _posSliderIncrement);
        PreciseLeftPosX = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionX);
    }

    [UIAction("precise-left-pos-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosXDecOnClick() {
        var newValue = StepDown(PreciseLeftPosX, _posSliderIncrement);
        PreciseLeftPosX = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionX);
    }

    #endregion

    #region PositionY

    private float _preciseLeftPosY;

    [UIValue("precise-left-pos-y-value")]
    [UsedImplicitly]
    private float PreciseLeftPosY {
        get => _preciseLeftPosY;
        set {
            if (_preciseLeftPosY.Equals(value)) return;
            _preciseLeftPosY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-pos-y-on-change")]
    [UsedImplicitly]
    private void PreciseLeftPosYOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftPosYTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.PositionY);
        } else {
            _preciseLeftPosY = value;
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionY);
        }
    }

    [UIAction("precise-left-pos-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosYIncOnClick() {
        var newValue = StepUp(PreciseLeftPosY, _posSliderIncrement);
        PreciseLeftPosY = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionY);
    }

    [UIAction("precise-left-pos-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosYDecOnClick() {
        var newValue = StepDown(PreciseLeftPosY, _posSliderIncrement);
        PreciseLeftPosY = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionY);
    }

    #endregion

    #region PositionZ

    private float _preciseLeftPosZ;

    [UIValue("precise-left-pos-z-value")]
    [UsedImplicitly]
    private float PreciseLeftPosZ {
        get => _preciseLeftPosZ;
        set {
            if (_preciseLeftPosZ.Equals(value)) return;
            _preciseLeftPosZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-pos-z-on-change")]
    [UsedImplicitly]
    private void PreciseLeftPosZOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftPosZTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.PositionZ);
        } else {
            _preciseLeftPosZ = value;
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionZ);
        }
    }

    [UIAction("precise-left-pos-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosZIncOnClick() {
        var newValue = StepUp(PreciseLeftPosZ, _posSliderIncrement);
        PreciseLeftPosZ = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionZ);
    }

    [UIAction("precise-left-pos-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftPosZDecOnClick() {
        var newValue = StepDown(PreciseLeftPosZ, _posSliderIncrement);
        PreciseLeftPosZ = ClampPosSliderValue(newValue);
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.PositionZ);
    }

    #endregion

    #region RotationX

    private float _preciseLeftRotX;

    [UIValue("precise-left-rot-x-value")]
    [UsedImplicitly]
    private float PreciseLeftRotX {
        get => _preciseLeftRotX;
        set {
            if (_preciseLeftRotX.Equals(value)) return;
            _preciseLeftRotX = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-x-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotXOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftRotXTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.RotationX);
        } else {
            _preciseLeftRotX = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationX);
        }
    }

    [UIAction("precise-left-rot-x-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotXIncOnClick() {
        var newValue = StepUp(PreciseLeftRotX, _rotSliderIncrement);
        PreciseLeftRotX = ClampRot90SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationX);
    }

    [UIAction("precise-left-rot-x-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotXDecOnClick() {
        var newValue = StepDown(PreciseLeftRotX, _rotSliderIncrement);
        PreciseLeftRotX = ClampRot90SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationX);
    }

    #endregion

    #region RotationY

    private float _preciseLeftRotY;

    [UIValue("precise-left-rot-y-value")]
    [UsedImplicitly]
    private float PreciseLeftRotY {
        get => _preciseLeftRotY;
        set {
            if (_preciseLeftRotY.Equals(value)) return;
            _preciseLeftRotY = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-y-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotYOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftRotYTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.RotationY);
        } else {
            _preciseLeftRotY = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationY);
        }
    }

    [UIAction("precise-left-rot-y-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotYIncOnClick() {
        var newValue = StepUp(PreciseLeftRotY, _rotSliderIncrement);
        PreciseLeftRotY = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationY);
    }

    [UIAction("precise-left-rot-y-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotYDecOnClick() {
        var newValue = StepDown(PreciseLeftRotY, _rotSliderIncrement);
        PreciseLeftRotY = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationY);
    }

    #endregion

    #region RotationZ

    private float _preciseLeftRotZ;

    [UIValue("precise-left-rot-z-value")]
    [UsedImplicitly]
    private float PreciseLeftRotZ {
        get => _preciseLeftRotZ;
        set {
            if (_preciseLeftRotZ.Equals(value)) return;
            _preciseLeftRotZ = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-z-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotZOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftRotZTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.RotationZ);
        } else {
            _preciseLeftRotZ = value;
            RecalculateReferenceSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationZ);
        }
    }

    [UIAction("precise-left-rot-z-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotZIncOnClick() {
        var newValue = StepUp(PreciseLeftRotZ, _rotSliderIncrement);
        PreciseLeftRotZ = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationZ);
    }

    [UIAction("precise-left-rot-z-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotZDecOnClick() {
        var newValue = StepDown(PreciseLeftRotZ, _rotSliderIncrement);
        PreciseLeftRotZ = ClampRot180SliderValue(newValue);
        RecalculateReferenceSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.RotationZ);
    }

    #endregion

    #region Curve

    private float _preciseLeftRotHor;

    [UIValue("precise-left-rot-hor-value")]
    [UsedImplicitly]
    private float PreciseLeftRotHor {
        get => _preciseLeftRotHor;
        set {
            if (_preciseLeftRotHor.Equals(value)) return;
            _preciseLeftRotHor = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-hor-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotHorOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftRotHorTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.Curve);
        } else {
            _preciseLeftRotHor = value;
            RecalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Curve);
        }
    }

    [UIAction("precise-left-rot-hor-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotHorIncOnClick() {
        var newValue = PreciseLeftRotHor + _rotSliderIncrement;
        PreciseLeftRotHor = ClampRot180SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Curve);
    }

    [UIAction("precise-left-rot-hor-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotHorDecOnClick() {
        var newValue = PreciseLeftRotHor - _rotSliderIncrement;
        PreciseLeftRotHor = ClampRot180SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Curve);
    }

    #endregion

    #region Balance

    private float _preciseLeftRotVert;

    [UIValue("precise-left-rot-vert-value")]
    [UsedImplicitly]
    private float PreciseLeftRotVert {
        get => _preciseLeftRotVert;
        set {
            if (_preciseLeftRotVert.Equals(value)) return;
            _preciseLeftRotVert = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("precise-left-rot-vert-on-change")]
    [UsedImplicitly]
    private void PreciseLeftRotVertOnChange(float value) {
        if (_smoothingEnabled) {
            _preciseLeftRotVertTarget = value;
            OnSliderTargetChanged(Hand.Left, SliderValueType.Balance);
        } else {
            _preciseLeftRotVert = value;
            RecalculateControllerSpaceRotations();
            OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Balance);
        }
    }

    [UIAction("precise-left-rot-vert-inc-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotVertIncOnClick() {
        var newValue = PreciseLeftRotVert + _rotSliderIncrement;
        PreciseLeftRotVert = ClampRot90SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Balance);
    }

    [UIAction("precise-left-rot-vert-dec-on-click")]
    [UsedImplicitly]
    private void PreciseLeftRotVertDecOnClick() {
        var newValue = PreciseLeftRotVert - _rotSliderIncrement;
        PreciseLeftRotVert = ClampRot90SliderValue(newValue);
        RecalculateControllerSpaceRotations();
        OnSliderValueChangedDirectly(Hand.Left, SliderValueType.Balance);
    }

    #endregion

    #region Interactable

    private bool _preciseLeftRotationReferenceInteractable = PluginConfig.LeftSaberHasReference;

    [UIValue("precise-left-rotation-reference-interactable")]
    [UsedImplicitly]
    private bool PreciseLeftRotationReferenceInteractable {
        get => _preciseLeftRotationReferenceInteractable;
        set {
            if (_preciseLeftRotationReferenceInteractable.Equals(value)) return;
            _preciseLeftRotationReferenceInteractable = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Help Active

    private bool _preciseLeftHelpActive;

    [UIValue("precise-left-help-active")]
    [UsedImplicitly]
    private bool PreciseLeftHelpActive {
        get => _preciseLeftHelpActive;
        set {
            if (_preciseLeftHelpActive.Equals(value)) return;
            _preciseLeftHelpActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Buttons Active

    private bool _preciseLeftButtonsActive;

    [UIValue("precise-left-buttons-active")]
    [UsedImplicitly]
    private bool PreciseLeftButtonsActive {
        get => _preciseLeftButtonsActive;
        set {
            if (_preciseLeftButtonsActive.Equals(value)) return;
            _preciseLeftButtonsActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region UpdateReference button

    [UIAction("precise-left-update-reference-on-click")]
    [UsedImplicitly]
    private void PreciseLeftUpdateReferenceOnClick() {
        PluginConfig.CreateUndoStep("Update left reference");
        PluginConfig.AlignLeftReferenceToCurrent();
    }

    #endregion

    #region ClearReference button

    [UIAction("precise-left-clear-reference-on-click")]
    [UsedImplicitly]
    private void PreciseLeftClearReferenceOnClick() {
        PluginConfig.CreateUndoStep("Clear left reference");
        PluginConfig.ResetLeftSaberReference();
    }

    #endregion
}