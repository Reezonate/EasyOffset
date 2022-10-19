using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage4 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Step 4 ‒ Fine-tuning";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "Rotation";
    public override string VideoUrl => RemoteConfig.UserGuideConfig.RotationVideoURL;

    #endregion
    
    #region Events

    [UIAction("rotation-on-click"), UsedImplicitly]
    private void RotationOnClick() {
        if (!PluginConfig.IsModPanelVisible) return;
        PluginConfig.AdjustmentMode = AdjustmentMode.Rotation;
    }

    #endregion
}