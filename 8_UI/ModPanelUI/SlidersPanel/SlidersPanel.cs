using System;

namespace EasyOffset;

internal partial class SlidersPanel : ReeUIComponentV2 {
    #region OnInitialize OnDispose

    protected override void OnInitialize() {
        ReeInputManager.PointerDownAction += OnPointerDown;
        ReeInputManager.PointerUpAction += OnPointerUp;
        PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        PluginConfig.ConfigWasChangedEvent += OnConfigWasChanged;
        PluginConfig.IsModPanelVisibleChangedEvent += OnIsModPanelVisibleChanged;
        OnConfigWasChanged();
    }

    protected override void OnDispose() {
        ReeInputManager.PointerDownAction -= OnPointerDown;
        ReeInputManager.PointerUpAction -= OnPointerUp;
        PluginConfig.ConfigWasChangedEvent -= OnConfigWasChanged;
        PluginConfig.IsModPanelVisibleChangedEvent -= OnIsModPanelVisibleChanged;
    }

    #endregion

    #region OnAdjustmentModeChanged

    private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        switch (adjustmentMode) {
            case AdjustmentMode.None:
                SetDirectPanelState(DirectPanelState.Hidden);
                break;
            case AdjustmentMode.Basic:
                SetDirectPanelState(DirectPanelState.ZOffsetOnly);
                break;
            case AdjustmentMode.Position:
                SetDirectPanelState(DirectPanelState.PositionOnly);
                break;
            case AdjustmentMode.Rotation:
            case AdjustmentMode.RotationAuto:
                SetDirectPanelState(DirectPanelState.RotationOnly);
                break;
            case AdjustmentMode.SwingBenchmark:
                SetDirectPanelState(DirectPanelState.Hidden);
                break;
            case AdjustmentMode.Direct:
                SetDirectPanelState(DirectPanelState.Full);
                break;
            case AdjustmentMode.RoomOffset:
                SetDirectPanelState(DirectPanelState.Hidden);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region Update

    private void Update() {
        SmoothingUpdate();
    }

    private void LateUpdate() {
        SynchronizationUpdate();
    }

    #endregion
}