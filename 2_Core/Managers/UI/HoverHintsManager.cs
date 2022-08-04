using System;
using HMUI;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset;

[UsedImplicitly]
internal class HoverHintsManager : IInitializable, IDisposable {
    [Inject, UsedImplicitly] private HoverHintController _hoverHintController;

    public void Initialize() {
        UIEvents.AdjustmentModeButtonWasPressedEvent += _hoverHintController.HideHintInstant;
    }

    public void Dispose() {
        UIEvents.AdjustmentModeButtonWasPressedEvent -= _hoverHintController.HideHintInstant;
    }
}