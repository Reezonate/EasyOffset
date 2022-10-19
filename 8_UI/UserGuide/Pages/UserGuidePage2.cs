using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage2 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Step 2 ‒ Rotations";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "RotationAuto";
    public override string VideoUrl => "https://github.com/Reezonate/EasyOffset/raw/experimental/media/RotationAuto.mp4";

    #endregion
    
    #region Events

    [UIAction("rotation-auto-on-click"), UsedImplicitly]
    private void RotationAutoOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.RotationAuto;

    #endregion
}