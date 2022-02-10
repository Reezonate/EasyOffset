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

    private float _legacyLeftPosXTarget, _legacyLeftPosXCurrent;
    private float _legacyLeftPosYTarget, _legacyLeftPosYCurrent;
    private float _legacyLeftPosZTarget, _legacyLeftPosZCurrent;
    private float _legacyLeftRotXTarget, _legacyLeftRotXCurrent;
    private float _legacyLeftRotYTarget, _legacyLeftRotYCurrent;

    private float _legacyRightPosXTarget, _legacyRightPosXCurrent;
    private float _legacyRightPosYTarget, _legacyRightPosYCurrent;
    private float _legacyRightPosZTarget, _legacyRightPosZCurrent;
    private float _legacyRightRotXTarget, _legacyRightRotXCurrent;
    private float _legacyRightRotYTarget, _legacyRightRotYCurrent;

    private void SmoothingLerpAll() {
        var holdTimeMultiplier = Mathf.Clamp01((Time.time - _smoothingStartTime) / 2.0f);
        var positionT = Time.deltaTime * 4f * holdTimeMultiplier;
        var xRotationT = positionT / 1.5f;
        var yRotationT = positionT / 3f;
        
        LegacyLeftPosX = _legacyLeftPosXCurrent = Mathf.Lerp(_legacyLeftPosXCurrent, _legacyLeftPosXTarget, positionT);
        LegacyLeftPosY = _legacyLeftPosYCurrent = Mathf.Lerp(_legacyLeftPosYCurrent, _legacyLeftPosYTarget, positionT);
        LegacyLeftPosZ = _legacyLeftPosZCurrent = Mathf.Lerp(_legacyLeftPosZCurrent, _legacyLeftPosZTarget, positionT);
        LegacyLeftRotX = _legacyLeftRotXCurrent = Mathf.Lerp(_legacyLeftRotXCurrent, _legacyLeftRotXTarget, xRotationT);
        LegacyLeftRotY = _legacyLeftRotYCurrent = Mathf.Lerp(_legacyLeftRotYCurrent, _legacyLeftRotYTarget, yRotationT);

        LegacyRightPosX = _legacyRightPosXCurrent = Mathf.Lerp(_legacyRightPosXCurrent, _legacyRightPosXTarget, positionT);
        LegacyRightPosY = _legacyRightPosYCurrent = Mathf.Lerp(_legacyRightPosYCurrent, _legacyRightPosYTarget, positionT);
        LegacyRightPosZ = _legacyRightPosZCurrent = Mathf.Lerp(_legacyRightPosZCurrent, _legacyRightPosZTarget, positionT);
        LegacyRightRotX = _legacyRightRotXCurrent = Mathf.Lerp(_legacyRightRotXCurrent, _legacyRightRotXTarget, xRotationT);
        LegacyRightRotY = _legacyRightRotYCurrent = Mathf.Lerp(_legacyRightRotYCurrent, _legacyRightRotYTarget, yRotationT);
    }

    private void SmoothingReset() {
        _legacyLeftPosXCurrent = _legacyLeftPosXTarget = LegacyLeftPosX;
        _legacyLeftPosYCurrent = _legacyLeftPosYTarget = LegacyLeftPosY;
        _legacyLeftPosZCurrent = _legacyLeftPosZTarget = LegacyLeftPosZ;
        _legacyLeftRotXCurrent = _legacyLeftRotXTarget = LegacyLeftRotX;
        _legacyLeftRotYCurrent = _legacyLeftRotYTarget = LegacyLeftRotY;

        _legacyRightPosXCurrent = _legacyRightPosXTarget = LegacyRightPosX;
        _legacyRightPosYCurrent = _legacyRightPosYTarget = LegacyRightPosY;
        _legacyRightPosZCurrent = _legacyRightPosZTarget = LegacyRightPosZ;
        _legacyRightRotXCurrent = _legacyRightRotXTarget = LegacyRightRotX;
        _legacyRightRotYCurrent = _legacyRightRotYTarget = LegacyRightRotY;
    }

    private void SmoothingFinalize() {
        LegacyLeftPosX = _legacyLeftPosXTarget = _legacyLeftPosXCurrent;
        LegacyLeftPosY = _legacyLeftPosYTarget = _legacyLeftPosYCurrent;
        LegacyLeftPosZ = _legacyLeftPosZTarget = _legacyLeftPosZCurrent;
        LegacyLeftRotX = _legacyLeftRotXTarget = _legacyLeftRotXCurrent;
        LegacyLeftRotY = _legacyLeftRotYTarget = _legacyLeftRotYCurrent;

        LegacyRightPosX = _legacyRightPosXTarget = _legacyRightPosXCurrent;
        LegacyRightPosY = _legacyRightPosYTarget = _legacyRightPosYCurrent;
        LegacyRightPosZ = _legacyRightPosZTarget = _legacyRightPosZCurrent;
        LegacyRightRotX = _legacyRightRotXTarget = _legacyRightRotXCurrent;
        LegacyRightRotY = _legacyRightRotYTarget = _legacyRightRotYCurrent;
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
        if (PluginConfig.AdjustmentMode != AdjustmentMode.Legacy) return;
        
        if (_smoothingEnabled) {
            if (_smoothValueWasChanged) SmoothingLerpAll();
            if (_applySmoothingUpdates) ApplyLegacyConfig();
        } else {
            if (_smoothingFinalized) return;
            _smoothingFinalized = true;
            SmoothingFinalize();
            ApplyLegacyConfig();
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