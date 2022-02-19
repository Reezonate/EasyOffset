using System;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Events

    public static event Action<Hand?> PreciseChangeStartedEvent;
    public static event Action<Hand?> PreciseChangeFinishedEvent;

    private void SubscribeToSmoothingEvents() {
        ReeInputManager.PointerDownAction += OnPointerDown;
        ReeInputManager.PointerUpAction += OnPointerUp;
    }

    private void OnPointerDown(ReeTriggerState triggerState) {
        OnSomethingWasPressed(triggerState);
    }

    private void OnPointerUp(ReeTriggerState triggerState) {
        OnSomethingWasReleased();
    }

    #endregion

    #region Smoothed Values

    private float _preciseLeftZOffsetTarget, _preciseLeftZOffsetCurrent;
    private float _preciseLeftPosXTarget, _preciseLeftPosXCurrent;
    private float _preciseLeftPosYTarget, _preciseLeftPosYCurrent;
    private float _preciseLeftPosZTarget, _preciseLeftPosZCurrent;
    private float _preciseLeftRotXTarget, _preciseLeftRotXCurrent;
    private float _preciseLeftRotYTarget, _preciseLeftRotYCurrent;
    private float _preciseLeftRotZTarget, _preciseLeftRotZCurrent;
    private float _preciseLeftRotHorTarget, _preciseLeftRotHorCurrent;
    private float _preciseLeftRotVertTarget, _preciseLeftRotVertCurrent;

    private float _preciseRightZOffsetTarget, _preciseRightZOffsetCurrent;
    private float _preciseRightPosXTarget, _preciseRightPosXCurrent;
    private float _preciseRightPosYTarget, _preciseRightPosYCurrent;
    private float _preciseRightPosZTarget, _preciseRightPosZCurrent;
    private float _preciseRightRotXTarget, _preciseRightRotXCurrent;
    private float _preciseRightRotYTarget, _preciseRightRotYCurrent;
    private float _preciseRightRotZTarget, _preciseRightRotZCurrent;
    private float _preciseRightRotHorTarget, _preciseRightRotHorCurrent;
    private float _preciseRightRotVertTarget, _preciseRightRotVertCurrent;

    #endregion

    #region SmoothingLerpAll

    private void SmoothingLerpAll() {
        var holdTimeMultiplier = Mathf.Clamp01((Time.time - _smoothingStartTime) / 2.0f);
        var positionT = Time.deltaTime * 4f * holdTimeMultiplier;
        var rotationT = positionT / 3f;

        LerpPositions(positionT);

        switch (_sliderValueType) {
            case SliderValueType.PositionX:
            case SliderValueType.PositionY:
            case SliderValueType.PositionZ:
            case SliderValueType.ZOffset:
                break;
            case SliderValueType.RotationX:
            case SliderValueType.RotationY:
            case SliderValueType.RotationZ:
                LerpRotationsSelf(rotationT);
                break;
            case SliderValueType.Curve:
            case SliderValueType.Balance:
                LerpRotationsReference(rotationT);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void LerpRotationsSelf(float t) {
        PreciseLeftRotX = _preciseLeftRotXCurrent = Mathf.Lerp(_preciseLeftRotXCurrent, _preciseLeftRotXTarget, t);
        PreciseLeftRotY = _preciseLeftRotYCurrent = Mathf.Lerp(_preciseLeftRotYCurrent, _preciseLeftRotYTarget, t);
        PreciseLeftRotZ = _preciseLeftRotZCurrent = Mathf.Lerp(_preciseLeftRotZCurrent, _preciseLeftRotZTarget, t);

        PreciseRightRotX = _preciseRightRotXCurrent = Mathf.Lerp(_preciseRightRotXCurrent, _preciseRightRotXTarget, t);
        PreciseRightRotY = _preciseRightRotYCurrent = Mathf.Lerp(_preciseRightRotYCurrent, _preciseRightRotYTarget, t);
        PreciseRightRotZ = _preciseRightRotZCurrent = Mathf.Lerp(_preciseRightRotZCurrent, _preciseRightRotZTarget, t);

        RecalculateReferenceSpaceRotations();
    }

    private void LerpRotationsReference(float t) {
        PreciseLeftRotHor = _preciseLeftRotHorCurrent = Mathf.Lerp(_preciseLeftRotHorCurrent, _preciseLeftRotHorTarget, t);
        PreciseLeftRotVert = _preciseLeftRotVertCurrent = Mathf.Lerp(_preciseLeftRotVertCurrent, _preciseLeftRotVertTarget, t);

        PreciseRightRotHor = _preciseRightRotHorCurrent = Mathf.Lerp(_preciseRightRotHorCurrent, _preciseRightRotHorTarget, t);
        PreciseRightRotVert = _preciseRightRotVertCurrent = Mathf.Lerp(_preciseRightRotVertCurrent, _preciseRightRotVertTarget, t);

        RecalculateControllerSpaceRotations();
    }

    private void LerpPositions(float t) {
        PreciseLeftZOffset = _preciseLeftZOffsetCurrent = Mathf.Lerp(_preciseLeftZOffsetCurrent, _preciseLeftZOffsetTarget, t);
        PreciseLeftPosX = _preciseLeftPosXCurrent = Mathf.Lerp(_preciseLeftPosXCurrent, _preciseLeftPosXTarget, t);
        PreciseLeftPosY = _preciseLeftPosYCurrent = Mathf.Lerp(_preciseLeftPosYCurrent, _preciseLeftPosYTarget, t);
        PreciseLeftPosZ = _preciseLeftPosZCurrent = Mathf.Lerp(_preciseLeftPosZCurrent, _preciseLeftPosZTarget, t);

        PreciseRightZOffset = _preciseRightZOffsetCurrent = Mathf.Lerp(_preciseRightZOffsetCurrent, _preciseRightZOffsetTarget, t);
        PreciseRightPosX = _preciseRightPosXCurrent = Mathf.Lerp(_preciseRightPosXCurrent, _preciseRightPosXTarget, t);
        PreciseRightPosY = _preciseRightPosYCurrent = Mathf.Lerp(_preciseRightPosYCurrent, _preciseRightPosYTarget, t);
        PreciseRightPosZ = _preciseRightPosZCurrent = Mathf.Lerp(_preciseRightPosZCurrent, _preciseRightPosZTarget, t);
    }

    #endregion

    #region SmoothingReset

    private void SmoothingReset() {
        _preciseLeftZOffsetCurrent = _preciseLeftZOffsetTarget = PreciseLeftZOffset;
        _preciseLeftPosXCurrent = _preciseLeftPosXTarget = PreciseLeftPosX;
        _preciseLeftPosYCurrent = _preciseLeftPosYTarget = PreciseLeftPosY;
        _preciseLeftPosZCurrent = _preciseLeftPosZTarget = PreciseLeftPosZ;
        _preciseLeftRotXCurrent = _preciseLeftRotXTarget = PreciseLeftRotX;
        _preciseLeftRotYCurrent = _preciseLeftRotYTarget = PreciseLeftRotY;
        _preciseLeftRotZCurrent = _preciseLeftRotZTarget = PreciseLeftRotZ;
        _preciseLeftRotHorCurrent = _preciseLeftRotHorTarget = PreciseLeftRotHor;
        _preciseLeftRotVertCurrent = _preciseLeftRotVertTarget = PreciseLeftRotVert;

        _preciseRightZOffsetCurrent = _preciseRightZOffsetTarget = PreciseRightZOffset;
        _preciseRightPosXCurrent = _preciseRightPosXTarget = PreciseRightPosX;
        _preciseRightPosYCurrent = _preciseRightPosYTarget = PreciseRightPosY;
        _preciseRightPosZCurrent = _preciseRightPosZTarget = PreciseRightPosZ;
        _preciseRightRotXCurrent = _preciseRightRotXTarget = PreciseRightRotX;
        _preciseRightRotYCurrent = _preciseRightRotYTarget = PreciseRightRotY;
        _preciseRightRotZCurrent = _preciseRightRotZTarget = PreciseRightRotZ;
        _preciseRightRotHorCurrent = _preciseRightRotHorTarget = PreciseRightRotHor;
        _preciseRightRotVertCurrent = _preciseRightRotVertTarget = PreciseRightRotVert;
    }

    #endregion

    #region SmoothingFinalize

    private void SmoothingFinalize() {
        PreciseLeftZOffset = _preciseLeftZOffsetTarget = _preciseLeftZOffsetCurrent;
        PreciseLeftPosX = _preciseLeftPosXTarget = _preciseLeftPosXCurrent;
        PreciseLeftPosY = _preciseLeftPosYTarget = _preciseLeftPosYCurrent;
        PreciseLeftPosZ = _preciseLeftPosZTarget = _preciseLeftPosZCurrent;
        PreciseLeftRotX = _preciseLeftRotXTarget = _preciseLeftRotXCurrent;
        PreciseLeftRotY = _preciseLeftRotYTarget = _preciseLeftRotYCurrent;
        PreciseLeftRotZ = _preciseLeftRotZTarget = _preciseLeftRotZCurrent;
        PreciseLeftRotHor = _preciseLeftRotHorTarget = _preciseLeftRotHorCurrent;
        PreciseLeftRotVert = _preciseLeftRotVertTarget = _preciseLeftRotVertCurrent;

        PreciseRightZOffset = _preciseRightZOffsetTarget = _preciseRightZOffsetCurrent;
        PreciseRightPosX = _preciseRightPosXTarget = _preciseRightPosXCurrent;
        PreciseRightPosY = _preciseRightPosYTarget = _preciseRightPosYCurrent;
        PreciseRightPosZ = _preciseRightPosZTarget = _preciseRightPosZCurrent;
        PreciseRightRotX = _preciseRightRotXTarget = _preciseRightRotXCurrent;
        PreciseRightRotY = _preciseRightRotYTarget = _preciseRightRotYCurrent;
        PreciseRightRotZ = _preciseRightRotZTarget = _preciseRightRotZCurrent;
        PreciseRightRotHor = _preciseRightRotHorTarget = _preciseRightRotHorCurrent;
        PreciseRightRotVert = _preciseRightRotVertTarget = _preciseRightRotVertCurrent;
    }

    #endregion

    #region Smoothing Flow

    private Hand _changeHand;
    private SliderValueType _sliderValueType = SliderValueType.ZOffset;
    private ReeTriggerState _clickedState = ReeTriggerState.Released;
    private bool _sliderValueWasChanged;
    private bool _applySmoothingUpdates;
    private bool _smoothingEnabled;
    private float _smoothingStartTime;

    private void SmoothingUpdate() {
        if (_precisePanelState == PrecisePanelState.Hidden) return;
        if (!_smoothingEnabled || !_sliderValueWasChanged) return;

        SmoothingLerpAll();
        if (_applySmoothingUpdates || _sliderValueType == SliderValueType.ZOffset) ApplyPreciseConfig();
    }

    private void OnSomethingWasPressed(ReeTriggerState triggerState) {
        _clickedState = triggerState;
        _smoothingEnabled = true;
        _sliderValueWasChanged = false;
        _applySmoothingUpdates = false;
    }

    private void OnSomethingWasReleased() {
        _smoothingEnabled = false;
        if (!_sliderValueWasChanged) return;
        _sliderValueWasChanged = false;
        SmoothingFinalize();
        ApplyPreciseConfig();
        PreciseChangeFinishedEvent?.Invoke(_changeHand);
    }

    private void OnSliderTargetChanged(Hand hand, SliderValueType sliderValueType) {
        if (_sliderValueWasChanged) return;

        _changeHand = hand;
        _sliderValueType = sliderValueType;
        _smoothingStartTime = Time.time;
        _sliderValueWasChanged = true;

        CreateSmoothValueUndoPoint(hand, sliderValueType);

        _applySmoothingUpdates = _clickedState switch {
            ReeTriggerState.Released => true,
            ReeTriggerState.LeftPressed => _changeHand != Hand.Left,
            ReeTriggerState.RightPressed => _changeHand != Hand.Right,
            _ => throw new ArgumentOutOfRangeException()
        };

        SmoothingReset();
        PreciseChangeStartedEvent?.Invoke(_changeHand);
    }

    private void OnSliderValueChangedDirectly(Hand hand, SliderValueType sliderValueType) {
        CreateSmoothValueUndoPoint(hand, sliderValueType);
        ApplyPreciseConfig();
    }

    private static void CreateSmoothValueUndoPoint(Hand hand, SliderValueType sliderValueType) {
        PluginConfig.CreateUndoStep($"Change {hand} {sliderValueType}");
    }

    #endregion

    #region Enums

    private enum SliderValueType {
        ZOffset,
        PositionX,
        PositionY,
        PositionZ,
        RotationX,
        RotationY,
        RotationZ,
        Curve,
        Balance
    }

    #endregion
}