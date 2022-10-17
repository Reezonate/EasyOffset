using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage4 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Step 4 ‒ Fine-tuning";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "Rotation";
    public override string VideoUrl => "https://github.com/Reezonate/EasyOffset/raw/experimental/media/Rotation.mp4";

    #endregion
    
    #region Events

    [UIAction("rotation-on-click"), UsedImplicitly]
    private void RotationOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.Rotation;

    #endregion
}