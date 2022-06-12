using System.Diagnostics;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class NonePanel : ReeUIComponentV2 {
    #region Initialize

    protected override void OnInitialize() {
        PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        PluginConfig.UILockChangedEvent += OnUILockChanged;
        UpdateActive();
    }

    #endregion

    #region Events

    private void OnUILockChanged(bool obj) {
        UpdateActive();
    }

    private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        UpdateActive();
    }

    private void UpdateActive() {
        NonePanelActive = !PluginConfig.UILock && PluginConfig.AdjustmentMode == AdjustmentMode.None;
    }

    #endregion

    #region Active

    private bool _nonePanelActive = true;

    [UIValue("none-panel-active"), UsedImplicitly]
    private bool NonePanelActive {
        get => _nonePanelActive;
        set {
            _nonePanelActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Guide

    [UIValue("none-panel-guide-text"), UsedImplicitly]
    private string _nonePanelText = "Select your controller in the \"Controller Type\" list" +
                                    "\nChoose a button you can press without changing your grip" +
                                    "\nHold the selected button and move your hand to make changes" +
                                    "\nEffect depends on a selected Adjustment Mode";

    #endregion

    #region Links

    [UIAction("none-panel-more-info-on-click"), UsedImplicitly]
    private void MoreInfoOnClick() {
        Process.Start("explorer.exe", "https://github.com/Reezonate/EasyOffset#how-to-use");
    }

    [UIAction("none-panel-donate-on-click"), UsedImplicitly]
    private void DonateOnClick() {
        Process.Start("explorer.exe", "https://ko-fi.com/reezonate");
    }

    #endregion
}