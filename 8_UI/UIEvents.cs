using System;

namespace EasyOffset;

internal static class UIEvents {
    #region AdjustmentModeButtonWasPressed

    public static event Action HoverHintUpdatedEvent;

    public static void NotifyHoverHintUpdated() {
        HoverHintUpdatedEvent?.Invoke();
    }

    #endregion
    
    #region AdjustmentModeButtonWasPressed

    public static event Action AdjustmentModeButtonWasPressedEvent;

    public static void NotifyAdjustmentModeButtonWasPressed() {
        AdjustmentModeButtonWasPressedEvent?.Invoke();
    }

    #endregion
    
    #region UserGuideButtonWasPressed

    public static event Action UserGuideButtonWasPressedEvent;

    public static void NotifyUserGuideButtonWasPressed() {
        UserGuideButtonWasPressedEvent?.Invoke();
    }

    #endregion
}