using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using JetBrains.Annotations;

namespace EasyOffset;

internal class AdjustmentModeSelectorModal : ReeUIComponentV2 {
    #region Init / Dispose

    protected override void OnInitialize() {
        InitializeModal();
        InitializePanels();

        UIEvents.AdjustmentModeButtonWasPressedEvent += ShowModal;
        PluginConfig.IsModPanelVisibleChangedEvent += OnIsModPanelVisibleChanged;
    }

    protected override void OnDispose() {
        UIEvents.AdjustmentModeButtonWasPressedEvent -= ShowModal;
        PluginConfig.IsModPanelVisibleChangedEvent -= OnIsModPanelVisibleChanged;
    }

    #endregion

    #region Events

    private void OnIsModPanelVisibleChanged(bool isVisible) {
        if (!isVisible) HideModal(false);
    }

    #endregion

    #region Panels

    [UIComponent("left-panel"), UsedImplicitly]
    private ImageView _leftPanel;

    [UIComponent("middle-panel"), UsedImplicitly]
    private ImageView _middlePanel;

    [UIComponent("right-panel"), UsedImplicitly]
    private ImageView _rightPanel;

    private void InitializePanels() {
        _leftPanel.raycastTarget = true;
        _rightPanel.raycastTarget = true;
        _middlePanel.raycastTarget = true;
    }

    #endregion

    #region Modal

    [UIComponent("modal"), UsedImplicitly]
    private ModalView _modal;

    private void InitializeModal() {
        var background = _modal.GetComponentInChildren<ImageView>();
        if (background != null) background.enabled = false;
        var touchable = _modal.GetComponentInChildren<Touchable>();
        if (touchable != null) touchable.enabled = false;
    }

    private void ShowModal(HoverHint hoverHint) {
        if (_modal == null) return;
        _modal.Show(true, true);
    }

    private void HideModal(bool animated) {
        if (_modal == null) return;
        _modal.Hide(animated);
    }

    #endregion

    #region Automatic

    [UIAction("position-auto-on-click"), UsedImplicitly]
    private void PositionAutoOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.PositionAuto;
        HideModal(true);
    }

    [UIAction("rotation-auto-on-click"), UsedImplicitly]
    private void RotationAutoOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.RotationAuto;
        HideModal(true);
    }

    #endregion

    #region Manual

    [UIAction("basic-on-click"), UsedImplicitly]
    private void BasicOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.Basic;
        HideModal(true);
    }

    [UIAction("position-on-click"), UsedImplicitly]
    private void PositionOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.Position;
        HideModal(true);
    }

    [UIAction("rotation-on-click"), UsedImplicitly]
    private void RotationOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.Rotation;
        HideModal(true);
    }

    [UIAction("none-on-click"), UsedImplicitly]
    private void NoneOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.None;
        HideModal(true);
    }

    #endregion

    #region Extras

    [UIAction("swing-benchmark-on-click"), UsedImplicitly]
    private void SwingBenchmarkOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.SwingBenchmark;
        HideModal(true);
    }

    [UIAction("direct-on-click"), UsedImplicitly]
    private void DirectOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.Direct;
        HideModal(true);
    }

    [UIAction("room-offset-on-click"), UsedImplicitly]
    private void RoomOffsetOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.RoomOffset;
        HideModal(true);
    }

    #endregion
}