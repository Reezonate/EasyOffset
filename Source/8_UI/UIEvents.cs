using HMUI;
using System;

namespace EasyOffset;

internal static class UIEvents {
    #region AdjustmentModeButtonWasPressed

    public static event Action<HoverHint> HoverHintUpdatedEvent;

    public static void NotifyHoverHintUpdated(HoverHint hoverHint) {
        HoverHintUpdatedEvent?.Invoke(hoverHint);
    }

    #endregion
    
    #region AdjustmentModeButtonWasPressed

    public static event Action<HoverHint> AdjustmentModeButtonWasPressedEvent;

    public static void NotifyAdjustmentModeButtonWasPressed(HoverHint hoverHint) {
        AdjustmentModeButtonWasPressedEvent?.Invoke(hoverHint);
    }

    #endregion
    
    #region UserGuideButtonWasPressed

    public static event Action UserGuideButtonWasPressedEvent;

    public static void NotifyUserGuideButtonWasPressed() {
        UserGuideButtonWasPressedEvent?.Invoke();
    }

    #endregion
    
    #region UserGuideVideoStarted

    public static event Action UserGuideVideoStartedEvent;

    public static void NotifyUserGuideVideoStarted() {
        UserGuideVideoStartedEvent?.Invoke();
    }

    #endregion
}