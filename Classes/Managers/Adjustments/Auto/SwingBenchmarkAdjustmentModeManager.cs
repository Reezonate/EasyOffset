using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class SwingBenchmarkAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        private readonly SwingBenchmarkManager _swingBenchmarkManager;

        public SwingBenchmarkAdjustmentModeManager(
            MainSettingsModelSO mainSettingsModel,
            SwingBenchmarkManager swingBenchmarkManager
        ) : base(mainSettingsModel,
            AdjustmentMode.SwingBenchmark,
            0f,
            0f
        ) {
            _swingBenchmarkManager = swingBenchmarkManager;
        }

        #endregion

        #region Logic

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            _swingBenchmarkManager.StartTracking();
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            GetAdjustmentHandWorldValues(
                adjustmentHand,
                adjustmentHandPos,
                adjustmentHandRot,
                out var tipPosition,
                out var pivotPosition,
                out var saberRotation
            );

            _swingBenchmarkManager.UpdateTracking(tipPosition, pivotPosition, saberRotation);
        }

        protected override void OnGrabFinished(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            _swingBenchmarkManager.StopTracking();
        }

        #endregion

        #region Utils

        private void GetAdjustmentHandWorldValues(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            out Vector3 tipWorldPosition,
            out Vector3 pivotWorldPosition,
            out Quaternion saberWorldRotation
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    pivotWorldPosition = TransformUtils.LocalToWorldVector(PluginConfig.LeftHandPivotPosition, adjustmentHandPos, adjustmentHandRot);
                    saberWorldRotation = adjustmentHandRot * PluginConfig.LeftHandRotation;
                    break;
                case Hand.Right:
                    pivotWorldPosition = TransformUtils.LocalToWorldVector(PluginConfig.RightHandPivotPosition, adjustmentHandPos, adjustmentHandRot);
                    saberWorldRotation = adjustmentHandRot * PluginConfig.RightHandRotation;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            TransformUtils.ApplyRoomOffset(MainSettingsModel, ref pivotWorldPosition, ref saberWorldRotation);
            tipWorldPosition = pivotWorldPosition + saberWorldRotation * Vector3.forward;
        }

        #endregion
    }
}