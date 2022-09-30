using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class UserGuidePage5 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "More info";
    public override bool ShowVideoPlayer => true;
    public override string VideoKey => "MoreInfo";
    public override string VideoUrl => "https://github.com/Reezonate/EasyOffset/raw/experimental/media/MoreInfo.mp4";
    public override bool IsFunny => true;

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