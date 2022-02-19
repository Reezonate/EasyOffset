using System;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Events

    public static event Action<Hand?> DirectChangeStartedEvent;
    public static event Action<Hand?> DirectChangeFinishedEvent;

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

    private float _directLeftZOffsetTarget, _directLeftZOffsetCurrent;
    private float _directLeftPosXTarget, _directLeftPosXCurrent;
    private float _directLeftPosYTarget, _directLeftPosYCurrent;
    private float _directLeftPosZTarget, _directLeftPosZCurrent;
    private float _directLeftRotXTarget, _directLeftRotXCurrent;
    private float _directLeftRotYTarget, _directLeftRotYCurrent;
    private float _directLeftRotZTarget, _directLeftRotZCurrent;
    private float _directLeftRotHorTarget, _directLeftRotHorCurrent;
    private float _directLeftRotVertTarget, _directLeftRotVertCurrent;

    private float _directRightZOffsetTarget, _directRightZOffsetCurrent;
    private float _directRightPosXTarget, _directRightPosXCurrent;
    private float _directRightPosYTarget, _directRightPosYCurrent;
    private float _directRightPosZTarget, _directRightPosZCurrent;
    private float _directRightRotXTarget, _directRightRotXCurrent;
    private float _directRightRotYTarget, _directRightRotYCurrent;
    private float _directRightRotZTarget, _directRightRotZCurrent;
    private float _directRightRotHorTarget, _directRightRotHorCurrent;
    private float _directRightRotVertTarget, _directRightRotVertCurrent;

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
        DirectLeftRotX = _directLeftRotXCurrent = Mathf.Lerp(_directLeftRotXCurrent, _directLeftRotXTarget, t);
        DirectLeftRotY = _directLeftRotYCurrent = Mathf.Lerp(_directLeftRotYCurrent, _directLeftRotYTarget, t);
        DirectLeftRotZ = _directLeftRotZCurrent = Mathf.Lerp(_directLeftRotZCurrent, _directLeftRotZTarget, t);

        DirectRightRotX = _directRightRotXCurrent = Mathf.Lerp(_directRightRotXCurrent, _directRightRotXTarget, t);
        DirectRightRotY = _directRightRotYCurrent = Mathf.Lerp(_directRightRotYCurrent, _directRightRotYTarget, t);
        DirectRightRotZ = _directRightRotZCurrent = Mathf.Lerp(_directRightRotZCurrent, _directRightRotZTarget, t);

        RecalculateReferenceSpaceRotations();
    }

    private void LerpRotationsReference(float t) {
        DirectLeftRotHor = _directLeftRotHorCurrent = Mathf.Lerp(_directLeftRotHorCurrent, _directLeftRotHorTarget, t);
        DirectLeftRotVert = _directLeftRotVertCurrent = Mathf.Lerp(_directLeftRotVertCurrent, _directLeftRotVertTarget, t);

        DirectRightRotHor = _directRightRotHorCurrent = Mathf.Lerp(_directRightRotHorCurrent, _directRightRotHorTarget, t);
        DirectRightRotVert = _directRightRotVertCurrent = Mathf.Lerp(_directRightRotVertCurrent, _directRightRotVertTarget, t);

        RecalculateControllerSpaceRotations();
    }

    private void LerpPositions(float t) {
        DirectLeftZOffset = _directLeftZOffsetCurrent = Mathf.Lerp(_directLeftZOffsetCurrent, _directLeftZOffsetTarget, t);
        DirectLeftPosX = _directLeftPosXCurrent = Mathf.Lerp(_directLeftPosXCurrent, _directLeftPosXTarget, t);
        DirectLeftPosY = _directLeftPosYCurrent = Mathf.Lerp(_directLeftPosYCurrent, _directLeftPosYTarget, t);
        DirectLeftPosZ = _directLeftPosZCurrent = Mathf.Lerp(_directLeftPosZCurrent, _directLeftPosZTarget, t);

        DirectRightZOffset = _directRightZOffsetCurrent = Mathf.Lerp(_directRightZOffsetCurrent, _directRightZOffsetTarget, t);
        DirectRightPosX = _directRightPosXCurrent = Mathf.Lerp(_directRightPosXCurrent, _directRightPosXTarget, t);
        DirectRightPosY = _directRightPosYCurrent = Mathf.Lerp(_directRightPosYCurrent, _directRightPosYTarget, t);
        DirectRightPosZ = _directRightPosZCurrent = Mathf.Lerp(_directRightPosZCurrent, _directRightPosZTarget, t);
    }

    #endregion

    #region SmoothingReset

    private void SmoothingReset() {
        _directLeftZOffsetCurrent = _directLeftZOffsetTarget = DirectLeftZOffset;
        _directLeftPosXCurrent = _directLeftPosXTarget = DirectLeftPosX;
        _directLeftPosYCurrent = _directLeftPosYTarget = DirectLeftPosY;
        _directLeftPosZCurrent = _directLeftPosZTarget = DirectLeftPosZ;
        _directLeftRotXCurrent = _directLeftRotXTarget = DirectLeftRotX;
        _directLeftRotYCurrent = _directLeftRotYTarget = DirectLeftRotY;
        _directLeftRotZCurrent = _directLeftRotZTarget = DirectLeftRotZ;
        _directLeftRotHorCurrent = _directLeftRotHorTarget = DirectLeftRotHor;
        _directLeftRotVertCurrent = _directLeftRotVertTarget = DirectLeftRotVert;

        _directRightZOffsetCurrent = _directRightZOffsetTarget = DirectRightZOffset;
        _directRightPosXCurrent = _directRightPosXTarget = DirectRightPosX;
        _directRightPosYCurrent = _directRightPosYTarget = DirectRightPosY;
        _directRightPosZCurrent = _directRightPosZTarget = DirectRightPosZ;
        _directRightRotXCurrent = _directRightRotXTarget = DirectRightRotX;
        _directRightRotYCurrent = _directRightRotYTarget = DirectRightRotY;
        _directRightRotZCurrent = _directRightRotZTarget = DirectRightRotZ;
        _directRightRotHorCurrent = _directRightRotHorTarget = DirectRightRotHor;
        _directRightRotVertCurrent = _directRightRotVertTarget = DirectRightRotVert;
    }

    #endregion

    #region SmoothingFinalize

    private void SmoothingFinalize() {
        DirectLeftZOffset = _directLeftZOffsetTarget = _directLeftZOffsetCurrent;
        DirectLeftPosX = _directLeftPosXTarget = _directLeftPosXCurrent;
        DirectLeftPosY = _directLeftPosYTarget = _directLeftPosYCurrent;
        DirectLeftPosZ = _directLeftPosZTarget = _directLeftPosZCurrent;
        DirectLeftRotX = _directLeftRotXTarget = _directLeftRotXCurrent;
        DirectLeftRotY = _directLeftRotYTarget = _directLeftRotYCurrent;
        DirectLeftRotZ = _directLeftRotZTarget = _directLeftRotZCurrent;
        DirectLeftRotHor = _directLeftRotHorTarget = _directLeftRotHorCurrent;
        DirectLeftRotVert = _directLeftRotVertTarget = _directLeftRotVertCurrent;

        DirectRightZOffset = _directRightZOffsetTarget = _directRightZOffsetCurrent;
        DirectRightPosX = _directRightPosXTarget = _directRightPosXCurrent;
        DirectRightPosY = _directRightPosYTarget = _directRightPosYCurrent;
        DirectRightPosZ = _directRightPosZTarget = _directRightPosZCurrent;
        DirectRightRotX = _directRightRotXTarget = _directRightRotXCurrent;
        DirectRightRotY = _directRightRotYTarget = _directRightRotYCurrent;
        DirectRightRotZ = _directRightRotZTarget = _directRightRotZCurrent;
        DirectRightRotHor = _directRightRotHorTarget = _directRightRotHorCurrent;
        DirectRightRotVert = _directRightRotVertTarget = _directRightRotVertCurrent;
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
        if (_directPanelState == DirectPanelState.Hidden) return;
        if (!_smoothingEnabled || !_sliderValueWasChanged) return;

        SmoothingLerpAll();
        if (_applySmoothingUpdates || _sliderValueType == SliderValueType.ZOffset) ApplyDirectConfig();
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
        ApplyDirectConfig();
        DirectChangeFinishedEvent?.Invoke(_changeHand);
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
        DirectChangeStartedEvent?.Invoke(_changeHand);
    }

    private void OnSliderValueChangedDirectly(Hand hand, SliderValueType sliderValueType) {
        CreateSmoothValueUndoPoint(hand, sliderValueType);
        ApplyDirectConfig();
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