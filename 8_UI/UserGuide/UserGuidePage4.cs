using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage4 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Step 4 ‒ Fine-tuning";
    public override bool ShowVideoPlayer => false;
    public override string VideoKey => default;
    public override string VideoUrl => default;

    #endregion
    
    #region Events

    [UIAction("rotation-on-click"), UsedImplicitly]
    private void RotationOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.Rotation;

    #endregion
}