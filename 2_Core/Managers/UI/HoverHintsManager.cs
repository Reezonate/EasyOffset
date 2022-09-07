using System;
using HMUI;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset;

[UsedImplicitly]
internal class HoverHintsManager : IInitializable, IDisposable {
    [Inject, UsedImplicitly] private HoverHintController _hoverHintController;
    [Inject, UsedImplicitly] private SongPreviewPlayer _songPreviewPlayer;

    public void Initialize() {
        UIEvents.HoverHintUpdatedEvent += _hoverHintController.HideHintInstant;
        UIEvents.AdjustmentModeButtonWasPressedEvent += _hoverHintController.HideHintInstant;
        UIEvents.UserGuideButtonWasPressedEvent += OnUserGuideWasOpened;
    }

    public void Dispose() {
        UIEvents.HoverHintUpdatedEvent -= _hoverHintController.HideHintInstant;
        UIEvents.AdjustmentModeButtonWasPressedEvent -= _hoverHintController.HideHintInstant;
        UIEvents.UserGuideButtonWasPressedEvent -= OnUserGuideWasOpened;
    }

    private void OnUserGuideWasOpened() {
        _hoverHintController.HideHintInstant();
        _songPreviewPlayer.FadeOut(0.6f);
    }
}