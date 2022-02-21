using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class PositionAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        private readonly GizmosManager _gizmosManager;

        public PositionAdjustmentModeManager(
            GizmosManager gizmosManager
        ) : base(
            AdjustmentMode.Position,
            3f,
            6f
        ) {
            _gizmosManager = gizmosManager;
        }

        #endregion

        #region Logic

        private Vector3 _grabWorldPosition;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            Vector3 storedLocalPosition;

            switch (adjustmentHand) {
                case Hand.Left:
                    storedLocalPosition = PluginConfig.LeftSaberPivotPosition;
                    _gizmosManager.LeftHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                case Hand.Right:
                    storedLocalPosition = PluginConfig.RightSaberPivotPosition;
                    _gizmosManager.RightHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _grabWorldPosition = adjustmentHandTransform.LocalToWorldPosition(storedLocalPosition);
            PluginConfig.CreateUndoStep($"Change {adjustmentHand} Position");
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            var pivotLocalPosition = adjustmentHandTransform.WorldToLocalPosition(_grabWorldPosition);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftSaberPivotPosition = pivotLocalPosition;
                    break;
                case Hand.Right:
                    PluginConfig.RightSaberPivotPosition = pivotLocalPosition;
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