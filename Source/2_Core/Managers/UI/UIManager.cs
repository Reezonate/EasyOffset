using System;
using HMUI;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset;

[UsedImplicitly]
internal class UIManager : IInitializable, IDisposable {
    [Inject, UsedImplicitly] private HoverHintController _hoverHintController;
    [Inject, UsedImplicitly] private SongPreviewPlayer _songPreviewPlayer;

    public void Initialize() {
        UIEvents.HoverHintUpdatedEvent += _hoverHintController.HideHintInstant;
        UIEvents.AdjustmentModeButtonWasPressedEvent += _hoverHintController.HideHintInstant;
        UIEvents.UserGuideButtonWasPressedEvent += MuteSongPreview;
        UIEvents.UserGuideVideoStartedEvent += MuteSongPreview;
    }

    public void Dispose() {
        UIEvents.HoverHintUpdatedEvent -= _hoverHintController.HideHintInstant;
        UIEvents.AdjustmentModeButtonWasPressedEvent -= _hoverHintController.HideHintInstant;
        UIEvents.UserGuideButtonWasPressedEvent -= MuteSongPreview;
        UIEvents.UserGuideVideoStartedEvent -= MuteSongPreview;
    }

    private void MuteSongPreview() {
        _songPreviewPlayer.FadeOut(0.6f);
    }
}