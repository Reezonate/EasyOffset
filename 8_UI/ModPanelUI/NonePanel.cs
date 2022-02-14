using System;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Active

    private bool _nonePanelActive = true;

    [UIValue("none-panel-active")]
    [UsedImplicitly]
    private bool NonePanelActive {
        get => _nonePanelActive;
        set {
            _nonePanelActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Guide

    [UIValue("none-panel-guide-text")] [UsedImplicitly]
    private string _nonePanelText = "- Select your controller in the \"Controller Type\" list" +
                                    "\n- Choose a button you can press without changing your grip" +
                                    "\n- Hold the selected button and move your hand to make changes" +
                                    "\n- Effect depends on a selected Adjustment Mode";

    #endregion

    #region Links

    [UIValue("none-panel-more-info-text")] [UsedImplicitly]
    private string _moreInfoText = "Click here for more information (the manual will open in your browser)";

    [UIAction("none-panel-more-info-on-click")]
    [UsedImplicitly]
    private void MoreInfoOnClick() {
        System.Diagnostics.Process.Start("explorer.exe", "https://github.com/Reezonate/EasyOffset#how-to-use");
    }

    #endregion
}