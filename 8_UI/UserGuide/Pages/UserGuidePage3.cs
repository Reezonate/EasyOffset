using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage3 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Step 3 ‒ Reference";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "SwingBenchmark";
    public override string VideoUrl => RemoteConfig.UserGuideConfig.SwingBenchmarkVideoURL;

    #endregion

    #region Events

    [UIAction("swing-benchmark-on-click"), UsedImplicitly]
    private void SwingBenchmarkOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.SwingBenchmark;

    #endregion
}