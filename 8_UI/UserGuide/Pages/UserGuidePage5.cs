using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class UserGuidePage5 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "More info";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "MoreInfo";
    public override string VideoUrl => RemoteConfig.UserGuideConfig.MoreInfoVideoURL;
    public override bool IsFunny => RemoteConfig.UserGuideConfig.FunnyEnabled;
    public override string WatchButtonText => "Get pro player configs";

    #endregion

    #region Events

    [UIAction("github-on-click"), UsedImplicitly]
    private void GuthubOnClick() {
        Application.OpenURL("https://github.com/Reezonate/EasyOffset#easy-offset");
    }

    [UIAction("discord-on-click"), UsedImplicitly]
    private void DiscordOnClick() {
        Application.OpenURL("https://discord.gg/HRdvMD2R8r");
    }

    #endregion
}