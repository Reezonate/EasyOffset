using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Events

    private void SubscribeToSmoothingEvents() {
        Abomination.ButtonPressedEvent += OnButtonPressed;
        Abomination.ButtonReleasedEvent += OnButtonReleased;
    }

    private void OnButtonPressed(Hand hand, ControllerButton controllerButton) {
        if (controllerButton != ControllerButton.TriggerButton) return;
        OnTriggerWasPressed(hand);
    }

    private void OnButtonReleased(Hand hand, ControllerButton controllerButton) {
        if (controllerButton != ControllerButton.TriggerButton) return;
        OnTriggerWasReleased();
    }

    #endregion

    #region SmoothValues

    private float _preciseLeftZOffsetTarget, _preciseLeftZOffsetCurrent;
    private float _preciseLeftPosXTarget, _preciseLeftPosXCurrent;
    private float _preciseLeftPosYTarget, _preciseLeftPosYCurrent;
    private float _preciseLeftPosZTarget, _preciseLeftPosZCurrent;
    private float _preciseLeftRotXTarget, _preciseLeftRotXCurrent;
    private float _preciseLeftRotYTarget, _preciseLeftRotYCurrent;

    private float _preciseRightZOffsetTarget, _preciseRightZOffsetCurrent;
    private float _preciseRightPosXTarget, _preciseRightPosXCurrent;
    private float _preciseRightPosYTarget, _preciseRightPosYCurrent;
    private float _preciseRightPosZTarget, _preciseRightPosZCurrent;
    private float _preciseRightRotXTarget, _preciseRightRotXCurrent;
    private float _preciseRightRotYTarget, _preciseRightRotYCurrent;

    private void SmoothingLerpAll() {
        var holdTimeMultiplier = Mathf.Clamp01((Time.time - _smoothingStartTime) / 2.0f);
        var positionT = Time.deltaTime * 4f * holdTimeMultiplier;
        var xRotationT = positionT / 1.5f;
        var yRotationT = positionT / 3f;

        PreciseLeftZOffset = _preciseLeftZOffsetCurrent = Mathf.Lerp(_preciseLeftZOffsetCurrent, _preciseLeftZOffsetTarget, positionT);
        PreciseLeftPosX = _preciseLeftPosXCurrent = Mathf.Lerp(_preciseLeftPosXCurrent, _preciseLeftPosXTarget, positionT);
        PreciseLeftPosY = _preciseLeftPosYCurrent = Mathf.Lerp(_preciseLeftPosYCurrent, _preciseLeftPosYTarget, positionT);
        PreciseLeftPosZ = _preciseLeftPosZCurrent = Mathf.Lerp(_preciseLeftPosZCurrent, _preciseLeftPosZTarget, positionT);
        PreciseLeftRotX = _preciseLeftRotXCurrent = Mathf.Lerp(_preciseLeftRotXCurrent, _preciseLeftRotXTarget, xRotationT);
        PreciseLeftRotY = _preciseLeftRotYCurrent = Mathf.Lerp(_preciseLeftRotYCurrent, _preciseLeftRotYTarget, yRotationT);

        PreciseRightZOffset = _preciseRightZOffsetCurrent = Mathf.Lerp(_preciseRightZOffsetCurrent, _preciseRightZOffsetTarget, positionT);
        PreciseRightPosX = _preciseRightPosXCurrent = Mathf.Lerp(_preciseRightPosXCurrent, _preciseRightPosXTarget, positionT);
        PreciseRightPosY = _preciseRightPosYCurrent = Mathf.Lerp(_preciseRightPosYCurrent, _preciseRightPosYTarget, positionT);
        PreciseRightPosZ = _preciseRightPosZCurrent = Mathf.Lerp(_preciseRightPosZCurrent, _preciseRightPosZTarget, positionT);
        PreciseRightRotX = _preciseRightRotXCurrent = Mathf.Lerp(_preciseRightRotXCurrent, _preciseRightRotXTarget, xRotationT);
        PreciseRightRotY = _preciseRightRotYCurrent = Mathf.Lerp(_preciseRightRotYCurrent, _preciseRightRotYTarget, yRotationT);
    }

    private void SmoothingReset() {
        _preciseLeftZOffsetCurrent = _preciseLeftZOffsetTarget = PreciseLeftZOffset;
        _preciseLeftPosXCurrent = _preciseLeftPosXTarget = PreciseLeftPosX;
        _preciseLeftPosYCurrent = _preciseLeftPosYTarget = PreciseLeftPosY;
        _preciseLeftPosZCurrent = _preciseLeftPosZTarget = PreciseLeftPosZ;
        _preciseLeftRotXCurrent = _preciseLeftRotXTarget = PreciseLeftRotX;
        _preciseLeftRotYCurrent = _preciseLeftRotYTarget = PreciseLeftRotY;

        _preciseRightZOffsetCurrent = _preciseRightZOffsetTarget = PreciseRightZOffset;
        _preciseRightPosXCurrent = _preciseRightPosXTarget = PreciseRightPosX;
        _preciseRightPosYCurrent = _preciseRightPosYTarget = PreciseRightPosY;
        _preciseRightPosZCurrent = _preciseRightPosZTarget = PreciseRightPosZ;
        _preciseRightRotXCurrent = _preciseRightRotXTarget = PreciseRightRotX;
        _preciseRightRotYCurrent = _preciseRightRotYTarget = PreciseRightRotY;
    }

    private void SmoothingFinalize() {
        PreciseLeftZOffset = _preciseLeftZOffsetTarget = _preciseLeftZOffsetCurrent;
        PreciseLeftPosX = _preciseLeftPosXTarget = _preciseLeftPosXCurrent;
        PreciseLeftPosY = _preciseLeftPosYTarget = _preciseLeftPosYCurrent;
        PreciseLeftPosZ = _preciseLeftPosZTarget = _preciseLeftPosZCurrent;
        PreciseLeftRotX = _preciseLeftRotXTarget = _preciseLeftRotXCurrent;
        PreciseLeftRotY = _preciseLeftRotYTarget = _preciseLeftRotYCurrent;

        PreciseRightZOffset = _preciseRightZOffsetTarget = _preciseRightZOffsetCurrent;
        PreciseRightPosX = _preciseRightPosXTarget = _preciseRightPosXCurrent;
        PreciseRightPosY = _preciseRightPosYTarget = _preciseRightPosYCurrent;
        PreciseRightPosZ = _preciseRightPosZTarget = _preciseRightPosZCurrent;
        PreciseRightRotX = _preciseRightRotXTarget = _preciseRightRotXCurrent;
        PreciseRightRotY = _preciseRightRotYTarget = _preciseRightRotYCurrent;
    }

    #endregion

    #region Smoothing Logic

    private Hand _clickedHand = Hand.Left;
    private bool _smoothValueWasChanged;
    private bool _applySmoothingUpdates;
    private bool _smoothingEnabled;
    private bool _smoothingFinalized = true;
    private float _smoothingStartTime;

    private void SmoothingUpdate() {
        if (PluginConfig.AdjustmentMode != AdjustmentMode.Precise) return;

        if (_smoothingEnabled) {
            if (_smoothValueWasChanged) SmoothingLerpAll();
            if (_applySmoothingUpdates) ApplyPreciseConfig();
        } else {
            if (_smoothingFinalized) return;
            _smoothingFinalized = true;
            SmoothingFinalize();
            ApplyPreciseConfig();
        }
    }

    private void OnTriggerWasPressed(Hand hand) {
        _clickedHand = hand;
        _smoothingEnabled = true;
        _smoothValueWasChanged = false;
    }

    private void OnTriggerWasReleased() {
        _smoothingEnabled = false;
    }

    private void OnSmoothValueChanged(Hand? hand) {
        if (_smoothValueWasChanged) return;
        SmoothingReset();
        _smoothingStartTime = Time.time;
        _smoothingFinalized = false;
        _smoothValueWasChanged = true;
        _applySmoothingUpdates = hand != _clickedHand;
    }

    #endregion
}