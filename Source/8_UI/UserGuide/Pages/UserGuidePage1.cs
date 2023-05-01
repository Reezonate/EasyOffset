using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage1 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Step 1 ‒ Positions";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "PositionAuto";
    public override string VideoUrl => RemoteConfig.UserGuideConfig.PositionAutoVideoURL;

    #endregion

    #region Events

    [UIAction("position-auto-on-click"), UsedImplicitly]
    private void PositionAutoOnClick() {
        if (!PluginConfig.IsModPanelVisible) return;
        PluginConfig.AdjustmentMode = AdjustmentMode.PositionAuto;
    }

    #endregion
}