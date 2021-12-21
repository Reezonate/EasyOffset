using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class RotationAutoAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        private readonly GizmosManager _gizmosManager;

        public RotationAutoAdjustmentModeManager(
            GizmosManager gizmosManager
        ) : base(
            AdjustmentMode.RotationAuto,
            0f,
            0f
        ) {
            _gizmosManager = gizmosManager;
        }

        #endregion

        #region Measurement logic

        private const int MaxMeasurementsCount = 60;
        private const float MinimalAngularVelocity = 45.0f;
        private const float MaximalAngularVelocity = 360.0f;

        private readonly RelativeRotationTracker _rotationTracker = new();
        private readonly Range _angularVelocityRange = new(MinimalAngularVelocity, MaximalAngularVelocity);
        private readonly WeightedList<Vector3> _measurements = new(MaxMeasurementsCount);

        private void Reset(ReeTransform controllerTransform) {
            _rotationTracker.Initialize(controllerTransform.Rotation);
            _measurements.Clear();
        }

        private bool Update(ReeTransform controllerTransform) {
            var mainCamera = Camera.main;
            var positiveDirection = mainCamera == null ? Vector3.forward : mainCamera.transform.forward;

            var updated = _rotationTracker.Update(controllerTransform.Rotation, positiveDirection, out var angle, out var axis);
            if (!updated) return false;

            var angularVelocity = angle / Time.deltaTime;
            if (angularVelocity <= MinimalAngularVelocity) return false;

            var axisLocal = controllerTransform.WorldToLocalDirection(axis);
            var weight = _angularVelocityRange.GetRatio(angularVelocity);

            _measurements.Add(axisLocal, weight);
            return true;
        }

        private Vector3 GetLocalAxis() {
            return _measurements.GetAverage();
        }

        #endregion

        #region Adjustment logic

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    _gizmosManager.LeftHandGizmosController.SetSphericalBasisFocus(true);
                    _gizmosManager.LeftHandGizmosController.SetPreviousDirection(PluginConfig.LeftHandSaberDirection, true);
                    break;
                case Hand.Right:
                    _gizmosManager.RightHandGizmosController.SetSphericalBasisFocus(true);
                    _gizmosManager.RightHandGizmosController.SetPreviousDirection(PluginConfig.RightHandSaberDirection, true);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            Reset(adjustmentHandTransform);
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            if (!Update(adjustmentHandTransform)) return;

            var localSaberDirection = GetLocalAxis().normalized;

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = localSaberDirection;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = localSaberDirection;
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
                    _gizmosManager.LeftHandGizmosController.SetPreviousDirection(PluginConfig.LeftHandSaberDirection, false);
                    break;
                case Hand.Right:
                    _gizmosManager.RightHandGizmosController.SetSphericalBasisFocus(false);
                    _gizmosManager.RightHandGizmosController.SetPreviousDirection(PluginConfig.RightHandSaberDirection, false);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }
        }

        #endregion
    }
}