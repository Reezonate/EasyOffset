using System;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class ResetAndMirrorControls : ReeUIComponentV2 {
    #region Constants

    private const float ButtonPromptDelaySeconds = 2.0f;
    private const string ButtonPromptText = "<color=#ff5555>Sure?</color>";

    private const string ResetButtonIdleText = "Reset";
    private const string LeftMirrorButtonIdleText = "Mirror to Right ►";
    private const string RightMirrorButtonIdleText = "<pos=-0.4em>◄ Mirror to Left";

    #endregion

    #region OnResetButtonPressed

    private static void OnResetButtonPressed(Hand hand) {
        switch (PluginConfig.AdjustmentMode) {
            case AdjustmentMode.None:
            case AdjustmentMode.SwingBenchmark:
            case AdjustmentMode.RoomOffset:
                return;
            case AdjustmentMode.Position:
            case AdjustmentMode.PositionAuto:
                PluginConfig.ResetOffsets(hand, true, false, false);
                break;
            case AdjustmentMode.Rotation:
            case AdjustmentMode.RotationAuto:
                PluginConfig.ResetOffsets(hand, false, true, true);
                break;
            case AdjustmentMode.Basic:
            case AdjustmentMode.Direct:
                PluginConfig.ResetOffsets(hand, true, true, false);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region OnMirrorButtonPressed

    private static void OnMirrorButtonPressed(Hand mirrorSource) {
        PluginConfig.Mirror(mirrorSource);
    }

    #endregion

    #region Left Reset Button

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

    #region Left Mirror Button

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

    #region Right Reset Button

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

    #region Right Mirror Button

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
}