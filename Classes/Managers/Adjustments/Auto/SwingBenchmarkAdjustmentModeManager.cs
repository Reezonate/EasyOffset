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
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            _swingBenchmarkManager.Reset(adjustmentHand);
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            GetAdjustmentHandWorldValues(
                adjustmentHand,
                adjustmentHandTransform,
                out var pivotWorldPosition,
                out var saberWorldRotation
            );

            _swingBenchmarkManager.Update(
                adjustmentHandTransform,
                pivotWorldPosition,
                saberWorldRotation
            );
        }

        protected override void OnGrabFinished(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            var saberLocalDirection = adjustmentHand switch {
                Hand.Left => PluginConfig.LeftHandSaberDirection,
                Hand.Right => PluginConfig.RightHandSaberDirection,
                _ => throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null)
            };

            _swingBenchmarkManager.Finalize(saberLocalDirection);
        }

        #endregion

        #region Utils

        private void GetAdjustmentHandWorldValues(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            out Vector3 pivotWorldPosition,
            out Quaternion saberWorldRotation
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    pivotWorldPosition = adjustmentHandTransform.LocalToWorldPosition(PluginConfig.LeftHandPivotPosition);
                    saberWorldRotation = adjustmentHandTransform.LocalToWorldRotation(PluginConfig.LeftHandRotation);
                    break;
                case Hand.Right:
                    pivotWorldPosition = adjustmentHandTransform.LocalToWorldPosition(PluginConfig.RightHandPivotPosition);
                    saberWorldRotation = adjustmentHandTransform.LocalToWorldRotation(PluginConfig.RightHandRotation);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            TransformUtils.ApplyRoomOffset(MainSettingsModel, ref pivotWorldPosition, ref saberWorldRotation);
        }

        #endregion
    }
}