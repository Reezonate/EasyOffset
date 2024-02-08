using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class PositionAutoAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        private readonly GizmosManager _gizmosManager;

        public PositionAutoAdjustmentModeManager(
            GizmosManager gizmosManager
        ) : base(
            AdjustmentMode.PositionAuto,
            0f,
            0f
        ) {
            _gizmosManager = gizmosManager;
        }

        #endregion

        #region Adjustment logic

        private const int MaxMeasurementsCount = 120;

        private readonly RotationPointTracker _tracker = new(MaxMeasurementsCount);

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    _gizmosManager.LeftHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                case Hand.Right:
                    _gizmosManager.RightHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _tracker.Clear();
            PluginConfig.CreateUndoStep($"Auto {adjustmentHand} Position");
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            _tracker.Update(adjustmentHandTransform.Position, adjustmentHandTransform.Rotation, Time.deltaTime);
            var origin =  _tracker.GetLocalOrigin();
            
            if (float.IsNaN(origin.x) || float.IsInfinity(origin.x)) return;
            if (float.IsNaN(origin.y) || float.IsInfinity(origin.y)) return;
            if (float.IsNaN(origin.z) || float.IsInfinity(origin.z)) return;

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftSaberPivotPosition = origin;
                    break;
                case Hand.Right:
                    PluginConfig.RightSaberPivotPosition = origin;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }
        }

        protected override void OnGrabFinished(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    _gizmosManager.LeftHandGizmosController.SetOrthonormalBasisFocus(false);
                    break;
                case Hand.Right:
                    _gizmosManager.RightHandGizmosController.SetOrthonormalBasisFocus(false);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }
        }

        #endregion
    }
}