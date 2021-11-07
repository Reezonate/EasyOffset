using System;

namespace EasyOffset {
    public static class SwingBenchmarkHelper {
        #region OnReset

        public static event Action OnResetEvent;

        public static void InvokeReset() {
            OnResetEvent?.Invoke();
        }

        #endregion

        #region OnStart

        public static event Action OnStartEvent;

        public static void InvokeStart() {
            OnStartEvent?.Invoke();
        }

        #endregion

        #region OnUpdate

        public delegate void OnUpdateDelegate(
            float swingCurveAngle,
            float tipDeviation,
            float pivotDeviation,
            float minimalSwingAngle,
            float maximalSwingAngle
        );

        public static event OnUpdateDelegate OnUpdateEvent;

        public static void InvokeUpdate(
            float swingCurveAngle,
            float tipDeviation,
            float pivotDeviation,
            float minimalSwingAngle,
            float maximalSwingAngle
        ) {
            OnUpdateEvent?.Invoke(
                swingCurveAngle,
                tipDeviation,
                pivotDeviation,
                minimalSwingAngle,
                maximalSwingAngle
            );
        }

        #endregion

        #region OnFail

        public static event Action OnFailEvent;

        public static void InvokeFail() {
            OnFailEvent?.Invoke();
        }

        #endregion

        #region OnSuccess

        public static event Action OnSuccessEvent;

        public static void InvokeSuccess() {
            OnSuccessEvent?.Invoke();
        }

        #endregion

        #region OnAutoFix

        public static event Action OnAutoFixEvent;

        public static void InvokeAutoFix() {
            OnAutoFixEvent?.Invoke();
        }

        #endregion
    }
}