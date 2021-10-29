using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class BasicAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        private readonly GizmosManager _gizmosManager;

        public BasicAdjustmentModeManager(
            MainSettingsModelSO mainSettingsModel,
            GizmosManager gizmosManager
        ) : base(
            mainSettingsModel,
            AdjustmentMode.Basic,
            4f,
            4f
        ) {
            _gizmosManager = gizmosManager;
        }

        #endregion

        #region Logic

        private Vector3 _storedLocalDirection;
        private Vector3 _grabWorldDirection;
        private Vector3 _grabPosition;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            Vector3 grabPivotPosition;

            switch (adjustmentHand) {
                case Hand.Left:
                    grabPivotPosition = PluginConfig.LeftHandPivotPosition;
                    _storedLocalDirection = PluginConfig.LeftHandSaberDirection;
                    _gizmosManager.LeftHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                case Hand.Right:
                    grabPivotPosition = PluginConfig.RightHandPivotPosition;
                    _storedLocalDirection = PluginConfig.RightHandSaberDirection;
                    _gizmosManager.RightHandGizmosController.SetOrthonormalBasisFocus(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _grabWorldDirection = adjustmentHandTransform.LocalToWorldDirection(_storedLocalDirection);
            _grabPosition = adjustmentHandTransform.LocalToWorldPosition(grabPivotPosition);
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            var finalLocalDirection = adjustmentHandTransform.WorldToLocalDirection(_grabWorldDirection);
            var newPivotPosition = adjustmentHandTransform.WorldToLocalPosition(_grabPosition);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = finalLocalDirection;
                    PluginConfig.LeftHandPivotPosition = newPivotPosition;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = finalLocalDirection;
                    PluginConfig.RightHandPivotPosition = newPivotPosition;
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