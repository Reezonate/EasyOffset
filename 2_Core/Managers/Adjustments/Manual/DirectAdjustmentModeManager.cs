using System;

namespace EasyOffset {
    internal static class DirectAdjustmentModeManager {
        public static event Action<Hand?> DirectChangeStartedEvent;
        public static event Action<Hand?> DirectChangeFinishedEvent;

        public static void NotifyDirectChangeStarted(Hand hand) {
            DirectChangeStartedEvent?.Invoke(hand);
        }

        public static void NotifyDirectChangeFinished(Hand hand) {
            DirectChangeFinishedEvent?.Invoke(hand);
        }
    }
}