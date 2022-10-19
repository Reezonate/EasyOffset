using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage0 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Getting Started";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "GettingStarted";
    public override string VideoUrl => "https://github.com/Reezonate/EasyOffset/raw/experimental/media/GettingStarted.mp4";
    public override string WatchButtonText => "Start watching";
    
    #endregion
}