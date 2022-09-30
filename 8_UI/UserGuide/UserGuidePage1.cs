using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage1 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Step 1 ‒ Positions";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "PositionAuto";
    public override string VideoUrl => "https://github.com/Reezonate/EasyOffset/raw/experimental/media/PositionAuto.mp4";

    #endregion

    #region Events

    [UIAction("position-auto-on-click"), UsedImplicitly]
    private void PositionAutoOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.PositionAuto;

    #endregion
}