using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class BottomPanel : ReeUIComponentV2 {
    #region Components

    [UIValue("undo-redo-buttons"), UsedImplicitly] private UndoRedoButtons _undoRedoButtons;
    [UIValue("warning-icon"), UsedImplicitly] private WarningIcon _warningIcon;

    private void Awake() {
        _undoRedoButtons = Instantiate<UndoRedoButtons>(transform, false);
        _warningIcon = Instantiate<WarningIcon>(transform, false);
    }

    #endregion

    #region Initialize & Dispose

    protected override void OnInitialize() {
        PluginConfig.IsModPanelVisibleChangedEvent += OnIsModPanelVisibleChanged;
        ApplyScale();
    }

    protected override void OnDispose() {
        PluginConfig.IsModPanelVisibleChangedEvent -= OnIsModPanelVisibleChanged;
    }

    #endregion

    #region Events

    private void OnIsModPanelVisibleChanged(bool value) {
        if (!value) return;
        ApplyScale();
    }

    #endregion

    #region Scale

    [UIComponent("undo-redo-buttons-container"), UsedImplicitly] private RectTransform _undoRedoButtonsContainer;
    [UIComponent("ui-lock-container"), UsedImplicitly] private RectTransform _uiLockContainer;

    private static readonly Vector3 UILockScale = Vector3.one * 0.85f;
    private static readonly Vector3 UndoRedoScale = Vector3.one * 0.95f;

    private void ApplyScale() {
        _undoRedoButtonsContainer.localScale = UndoRedoScale;
        _uiLockContainer.localScale = UILockScale;
    }

    #endregion

    #region Save & Load buttons

    [UIAction("bp-save-on-click"), UsedImplicitly]
    private void BottomPanelSaveOnClick() {
        ModPanelUI.OpenBrowserPage(true, false);
    }

    [UIAction("bp-load-on-click"), UsedImplicitly]
    private void BottomPanelLoadOnClick() {
        ModPanelUI.OpenBrowserPage(false, true);
    }

    #endregion

    #region UserGuide

    [UIAction("user-guide-on-click"), UsedImplicitly]
    private void UserGuideOnClick() {
        UIEvents.NotifyUserGuideButtonWasPressed();
    }

    #endregion

    #region UI Lock

    private AdjustmentMode _previousAdjustmentMode;

    [UIValue("interactable"), UsedImplicitly]
    private bool Interactable {
        get => !PluginConfig.UILock;
        set {
            PluginConfig.UILock = !value;

            if (!value) {
                _previousAdjustmentMode = PluginConfig.AdjustmentMode;
                PluginConfig.AdjustmentMode = AdjustmentMode.None;
            } else {
                PluginConfig.AdjustmentMode = _previousAdjustmentMode;
            }

            ModPanelUI.OpenMainPage();
            NotifyPropertyChanged();
        }
    }

    [UIValue("lock-value"), UsedImplicitly] private bool _lockValue = PluginConfig.UILock;

    [UIAction("lock-on-change"), UsedImplicitly]
    private void LockOnChange(bool value) {
        Interactable = !value;
    }

    #endregion
}