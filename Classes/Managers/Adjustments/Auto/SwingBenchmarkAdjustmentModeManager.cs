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
            _swingBenchmarkManager.Start(adjustmentHand);
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            GetHandWorldValues(
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
            _swingBenchmarkManager.Finish();
        }

        #endregion

        #region Utils

        private void GetHandWorldValues(
            Hand hand,
            ReeTransform controllerTransform,
            out Vector3 pivotWorldPosition,
            out Quaternion saberWorldRotation
        ) {
            switch (hand) {
                case Hand.Left:
                    pivotWorldPosition = controllerTransform.LocalToWorldPosition(PluginConfig.LeftHandPivotPosition);
                    saberWorldRotation = controllerTransform.LocalToWorldRotation(PluginConfig.LeftHandRotation);
                    break;
                case Hand.Right:
                    pivotWorldPosition = controllerTransform.LocalToWorldPosition(PluginConfig.RightHandPivotPosition);
                    saberWorldRotation = controllerTransform.LocalToWorldRotation(PluginConfig.RightHandRotation);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }

            TransformUtils.ApplyRoomOffset(MainSettingsModel, ref pivotWorldPosition, ref saberWorldRotation);
        }

        #endregion
    }
}