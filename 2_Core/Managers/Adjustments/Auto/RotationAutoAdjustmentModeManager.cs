using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;

namespace EasyOffset {
    [UsedImplicitly]
    public class RotationAutoAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constants

        private const int MinimalCapacity = 60;
        private const int MaximalCapacity = 300;
        private const float ProbeTime = 2.0f;
        private const float MinimalAngularVelocity = 45.0f;
        private const float MaximalAngularVelocity = 360.0f;

        #endregion

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

            var capacity = (int)Mathf.Clamp(XRDevice.refreshRate * ProbeTime, MinimalCapacity, MaximalCapacity);
            _measurements = new WeightedList<Vector3>(capacity);
        }

        #endregion

        #region Measurement logic

        private readonly RelativeRotationTracker _rotationTracker = new();
        private readonly Range _angularVelocityRange = new(MinimalAngularVelocity, MaximalAngularVelocity);
        private readonly WeightedList<Vector3> _measurements;

        private void Reset(ReeTransform controllerTransform) {
            _rotationTracker.Initialize(controllerTransform.Rotation);
            _measurements.Clear();
        }

        private bool Update(ReeTransform controllerTransform) {
            var mainCamera = Camera.main;
            var positiveDirection = mainCamera == null ? Vector3.forward : mainCamera.transform.forward;

            var updated = _rotationTracker.Update(
                controllerTransform.Rotation, positiveDirection, Time.deltaTime,
                out var angularVelocity, out var axis
            );

            if (!updated || angularVelocity <= MinimalAngularVelocity) return false;

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

        private Quaternion _grabLocalRotation;
        private Vector3 _grabLocalDirection;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    _grabLocalRotation = PluginConfig.LeftSaberRotation;
                    _gizmosManager.LeftHandGizmosController.SetSphericalBasisFocus(true);
                    break;
                case Hand.Right:
                    _grabLocalRotation = PluginConfig.RightSaberRotation;
                    _gizmosManager.RightHandGizmosController.SetSphericalBasisFocus(true);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _grabLocalDirection = TransformUtils.DirectionFromRotation(_grabLocalRotation);
            Reset(adjustmentHandTransform);
            PluginConfig.CreateUndoStep($"Auto {adjustmentHand} Rotation");
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            if (!Update(adjustmentHandTransform)) return;

            var localSaberDirection = GetLocalAxis().normalized;
            var finalRotation = Quaternion.FromToRotation(_grabLocalDirection, localSaberDirection) * _grabLocalRotation;

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftSaberRotation = finalRotation;
                    break;
                case Hand.Right:
                    PluginConfig.RightSaberRotation = finalRotation;
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
                    break;
                case Hand.Right:
                    _gizmosManager.RightHandGizmosController.SetSphericalBasisFocus(false);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }
        }

        #endregion
    }
}