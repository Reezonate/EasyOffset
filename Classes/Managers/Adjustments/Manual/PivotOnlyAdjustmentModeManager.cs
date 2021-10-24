using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class PivotOnlyAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        private readonly GizmosManager _gizmosManager;

        public PivotOnlyAdjustmentModeManager(
            MainSettingsModelSO mainSettingsModelSO,
            GizmosManager gizmosManager
        ) : base(
            mainSettingsModelSO,
            AdjustmentMode.PivotOnly,
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
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            Vector3 storedLocalPosition;

            switch (adjustmentHand) {
                case Hand.Left:
                    storedLocalPosition = PluginConfig.LeftHandPivotPosition;
                    _gizmosManager.LeftHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                case Hand.Right:
                    storedLocalPosition = PluginConfig.RightHandPivotPosition;
                    _gizmosManager.RightHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _grabWorldPosition = TransformUtils.LocalToWorldVector(storedLocalPosition, adjustmentHandPos, adjustmentHandRot);
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            var currentPivotLocalPosition = TransformUtils.WorldToLocalVector(_grabWorldPosition, adjustmentHandPos, adjustmentHandRot);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftHandPivotPosition = currentPivotLocalPosition;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandPivotPosition = currentPivotLocalPosition;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }
        }

        protected override void OnGrabFinished(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
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