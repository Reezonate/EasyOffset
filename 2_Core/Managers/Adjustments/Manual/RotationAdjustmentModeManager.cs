using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class RotationAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        private readonly GizmosManager _gizmosManager;

        public RotationAdjustmentModeManager(
            GizmosManager gizmosManager
        ) : base(
            AdjustmentMode.Rotation,
            6f,
            3f
        ) {
            _gizmosManager = gizmosManager;
        }

        #endregion

        #region Logic

        private Quaternion _grabWorldRotation;

        private readonly Range _heightRange = new Range(0.05f, 0.4f);
        private readonly Range _zoomRange = new Range(1f, 4f);
        private float _grabFreeY;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            Quaternion grabLocalRotation;

            switch (adjustmentHand) {
                case Hand.Left:
                    grabLocalRotation = PluginConfig.LeftSaberRotation;
                    _gizmosManager.LeftHandGizmosController.SetSphericalBasisFocus(true);
                    _gizmosManager.LeftHandGizmosController.SetPreviousRotation(grabLocalRotation, true);
                    break;
                case Hand.Right:
                    grabLocalRotation = PluginConfig.RightSaberRotation;
                    _gizmosManager.RightHandGizmosController.SetSphericalBasisFocus(true);
                    _gizmosManager.RightHandGizmosController.SetPreviousRotation(grabLocalRotation, true);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _grabWorldRotation = adjustmentHandTransform.LocalToWorldRotation(grabLocalRotation);
            _grabFreeY = freeHandTransform.Position.y;
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            var finalLocalRotation = adjustmentHandTransform.WorldToLocalRotation(_grabWorldRotation);

            var heightDifference = freeHandTransform.Position.y - _grabFreeY;
            var zoomRatio = _heightRange.GetRatioClamped(heightDifference);
            var zoom = _zoomRange.SlideBy(zoomRatio);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftSaberRotation = finalLocalRotation;
                    _gizmosManager.LeftHandGizmosController.Zoom(zoom);
                    break;
                case Hand.Right:
                    PluginConfig.RightSaberRotation = finalLocalRotation;
                    _gizmosManager.RightHandGizmosController.Zoom(zoom);
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
                    _gizmosManager.LeftHandGizmosController.SetSphericalBasisFocus(false);
                    _gizmosManager.LeftHandGizmosController.SetPreviousRotation(PluginConfig.LeftSaberRotation, false);
                    _gizmosManager.LeftHandGizmosController.Zoom(1);
                    break;
                case Hand.Right:
                    _gizmosManager.RightHandGizmosController.SetSphericalBasisFocus(false);
                    _gizmosManager.RightHandGizmosController.SetPreviousRotation(PluginConfig.RightSaberRotation, false);
                    _gizmosManager.RightHandGizmosController.Zoom(1);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }
        }

        #endregion
    }
}