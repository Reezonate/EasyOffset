using System;

namespace EasyOffset;

internal static class UIEvents {
    #region AdjustmentModeButtonWasPressed

    public static event Action AdjustmentModeButtonWasPressedEvent;

    public static void NotifyAdjustmentModeButtonWasPressed() {
        AdjustmentModeButtonWasPressedEvent?.Invoke();
    }

    #endregion
}