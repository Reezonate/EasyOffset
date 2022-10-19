using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class ReferenceControls : ReeUIComponentV2 {
    #region Initialize

    protected override void OnInitialize() {
        PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        PluginConfig.ConfigWasChangedEvent += OnConfigWasChanged;
        OnAdjustmentModeChanged(PluginConfig.AdjustmentMode);
        OnConfigWasChanged();
    }

    protected override void OnDispose() {
        PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
        PluginConfig.ConfigWasChangedEvent -= OnConfigWasChanged;
    }

    #endregion

    #region Events

    private void OnConfigWasChanged() {
        DirectLeftButtonsActive = PluginConfig.LeftSaberHasReference;
        DirectLeftHelpActive = !_directLeftButtonsActive;

        DirectRightButtonsActive = PluginConfig.RightSaberHasReference;
        DirectRightHelpActive = !_directRightButtonsActive;
    }

    private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        IsActive = adjustmentMode is AdjustmentMode.Rotation or AdjustmentMode.RotationAuto;
    }

    [UIAction("help-on-click"), UsedImplicitly]
    private void HelpOnClick() {
        PluginConfig.AdjustmentMode = AdjustmentMode.SwingBenchmark;
    }

    #endregion

    #region IsActive

    private bool _isActive;

    [UIValue("is-active"), UsedImplicitly]
    private bool IsActive {
        get => _isActive;
        set {
            if (_isActive.Equals(value)) return;
            _isActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Left

    #region Help Active

    private bool _directLeftHelpActive;

    [UIValue("direct-left-help-active"), UsedImplicitly]
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

    [UIValue("direct-left-buttons-active"), UsedImplicitly]
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

    [UIAction("direct-left-update-reference-on-click"), UsedImplicitly]
    private void DirectLeftUpdateReferenceOnClick() {
        PluginConfig.CreateUndoStep("Update left reference");
        PluginConfig.AlignLeftReferenceToCurrent();
    }

    #endregion

    #region ClearReference button

    [UIAction("direct-left-clear-reference-on-click"), UsedImplicitly]
    private void DirectLeftClearReferenceOnClick() {
        PluginConfig.CreateUndoStep("Clear left reference");
        PluginConfig.ResetLeftSaberReference();
    }

    #endregion

    #endregion

    #region Right

    #region Help Active

    private bool _directRightHelpActive;

    [UIValue("direct-right-help-active"), UsedImplicitly]
    private bool DirectRightHelpActive {
        get => _directRightHelpActive;
        set {
            if (_directRightHelpActive.Equals(value)) return;
            _directRightHelpActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Buttons Active

    private bool _directRightButtonsActive;

    [UIValue("direct-right-buttons-active"), UsedImplicitly]
    private bool DirectRightButtonsActive {
        get => _directRightButtonsActive;
        set {
            if (_directRightButtonsActive.Equals(value)) return;
            _directRightButtonsActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region UpdateReference button

    [UIAction("direct-right-update-reference-on-click"), UsedImplicitly]
    private void DirectRightUpdateReferenceOnClick() {
        PluginConfig.CreateUndoStep("Update right reference");
        PluginConfig.AlignRightReferenceToCurrent();
    }

    #endregion

    #region ClearReference button

    [UIAction("direct-right-clear-reference-on-click"), UsedImplicitly]
    private void DirectRightClearReferenceOnClick() {
        PluginConfig.CreateUndoStep("Clear right reference");
        PluginConfig.ResetRightSaberReference();
    }

    #endregion

    #endregion
}