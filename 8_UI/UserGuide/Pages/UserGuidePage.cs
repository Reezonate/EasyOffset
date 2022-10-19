using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal abstract class UserGuidePage: ReeUIComponentV2 {
    public abstract string Title { get; }
    public virtual bool ShowVideoPlayer { get; } = false;
    public virtual string VideoKey { get; } = "";
    public virtual string VideoUrl { get; } = "";
    public virtual bool IsFunny { get; } = false;
    
    public virtual string WatchButtonText { get; } = "Watch video";
    public virtual string CloseButtonText { get; } = "Back";

    [UIObject("root-object"), UsedImplicitly]
    private protected GameObject _rootObject;

    public void SetActive(bool value) {
        _rootObject.SetActive(value);
    }
}