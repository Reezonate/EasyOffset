using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class BasicAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        private readonly GizmosManager _gizmosManager;

        public BasicAdjustmentModeManager(GizmosManager gizmosManager) : base(
            AdjustmentMode.Basic,
            4f,
            4f
        ) {
            _gizmosManager = gizmosManager;
        }

        #endregion

        #region Logic

        private Quaternion _grabWorldRotation;
        private Vector3 _grabPosition;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            Quaternion grabLocalRotation;
            Vector3 grabPivotPosition;

            switch (adjustmentHand) {
                case Hand.Left:
                    grabPivotPosition = PluginConfig.LeftSaberPivotPosition;
                    grabLocalRotation = PluginConfig.LeftSaberRotation;
                    _gizmosManager.LeftHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                case Hand.Right:
                    grabPivotPosition = PluginConfig.RightSaberPivotPosition;
                    grabLocalRotation = PluginConfig.RightSaberRotation;
                    _gizmosManager.RightHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _grabWorldRotation = adjustmentHandTransform.LocalToWorldRotation(grabLocalRotation);
            _grabPosition = adjustmentHandTransform.LocalToWorldPosition(grabPivotPosition);
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            var finalLocalRotation = adjustmentHandTransform.WorldToLocalRotation(_grabWorldRotation);
            var newPivotPosition = adjustmentHandTransform.WorldToLocalPosition(_grabPosition);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftSaberRotation = finalLocalRotation;
                    PluginConfig.LeftSaberPivotPosition = newPivotPosition;
                    break;
                case Hand.Right:
                    PluginConfig.RightSaberRotation = finalLocalRotation;
                    PluginConfig.RightSaberPivotPosition = newPivotPosition;
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