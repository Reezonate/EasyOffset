namespace EasyOffset;

internal class UserGuidePage0 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Getting Started";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "GettingStarted";
    public override string VideoUrl => RemoteConfig.UserGuideConfig.GettingStartedVideoURL;
    public override string WatchButtonText => "Start watching";

    #endregion
}