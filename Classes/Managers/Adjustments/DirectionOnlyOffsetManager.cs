using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class DirectionOnlyOffsetManager : AbstractOffsetManager {
        #region Constructor

        private readonly GizmosManager _gizmosManager;

        public DirectionOnlyOffsetManager(
            GizmosManager gizmosManager,
            MainSettingsModelSO mainSettingsModel
        ) : base(
            mainSettingsModel,
            AdjustmentMode.DirectionOnly,
            6f,
            3f
        ) {
            _gizmosManager = gizmosManager;
        }

        #endregion

        #region Logic

        private Vector3 _storedLocalDirection;
        private Vector3 _grabWorldDirection;

        private readonly Range _heightRange = new Range(0.05f, 0.4f);
        private readonly Range _zoomRange = new Range(1f, 4f);
        private float _grabFreeY;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    _storedLocalDirection = PluginConfig.LeftHandSaberDirection;
                    _gizmosManager.LeftHandGizmosController.SetSphericalBasisFocus(true);
                    _gizmosManager.LeftHandGizmosController.SetPreviousDirection(_storedLocalDirection, true);
                    break;
                case Hand.Right:
                    _storedLocalDirection = PluginConfig.RightHandSaberDirection;
                    _gizmosManager.RightHandGizmosController.SetSphericalBasisFocus(true);
                    _gizmosManager.RightHandGizmosController.SetPreviousDirection(_storedLocalDirection, true);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _grabWorldDirection = adjustmentHandRot * _storedLocalDirection;
            _grabFreeY = freeHandPos.y;
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            var finalLocalDirection = Quaternion.Inverse(adjustmentHandRot) * _grabWorldDirection;

            var heightDifference = freeHandPos.y - _grabFreeY;
            var zoomRatio = _heightRange.GetRatioClamped(heightDifference);
            var zoom = _zoomRange.SlideBy(zoomRatio);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = finalLocalDirection;
                    _gizmosManager.LeftHandGizmosController.Zoom(zoom);
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = finalLocalDirection;
                    _gizmosManager.RightHandGizmosController.Zoom(zoom);
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
                    _gizmosManager.LeftHandGizmosController.SetSphericalBasisFocus(false);
                    _gizmosManager.LeftHandGizmosController.SetPreviousDirection(PluginConfig.LeftHandSaberDirection, false);
                    _gizmosManager.LeftHandGizmosController.Zoom(1);
                    break;
                case Hand.Right:
                    _gizmosManager.RightHandGizmosController.SetSphericalBasisFocus(false);
                    _gizmosManager.RightHandGizmosController.SetPreviousDirection(PluginConfig.RightHandSaberDirection, false);
                    _gizmosManager.RightHandGizmosController.Zoom(1);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }
        }

        #endregion
    }
}