using System;
using System.Collections.Generic;
using System.Linq;
using EasyOffset.AssetBundleScripts;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class DirectionAutoAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constants

        private const int MaxMeasurementsCount = 60;
        private const int PlaneTrackerMeasurements = 6;
        private const float MinimalAngularVelocity = 45.0f;
        private const float MaximalAngularVelocity = 360.0f;

        #endregion

        #region Constructor

        private readonly RotationPlaneTracker _rotationPlaneTracker;

        public DirectionAutoAdjustmentModeManager(
            MainSettingsModelSO mainSettingsModel
        ) : base(
            mainSettingsModel,
            AdjustmentMode.DirectionAuto,
            4f,
            10f
        ) {
            _rotationPlaneTracker = new RotationPlaneTracker(
                BundleLoader.SphereMesh,
                PlaneTrackerMeasurements,
                MinimalAngularVelocity,
                MaximalAngularVelocity
            );
        }

        #endregion

        #region Measurements and averaging

        private readonly List<Vector3> _measurements = new();

        private void ResetMeasurements() {
            _measurements.Clear();
        }

        private void RecordNewMeasurement(Vector3 direction) {
            _measurements.Add(direction);
            if (_measurements.Count > MaxMeasurementsCount) _measurements.RemoveAt(0);
        }

        private Vector3 GetAverageMeasurement() {
            var result = _measurements.Aggregate(Vector3.zero, (current, measurement) => current + measurement);
            return result / _measurements.Count;
        }

        #endregion

        #region AdjustmentLogic

        private Quaternion _grabLocalRotation;


        protected override void OnGrabStarted(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            _grabLocalRotation = adjustmentHand switch {
                Hand.Left => PluginConfig.LeftHandRotation,
                Hand.Right => PluginConfig.RightHandRotation,
                _ => throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null)
            };

            ResetMeasurements();
            RecordNewMeasurement(_grabLocalRotation * Vector3.forward);

            var worldRotation = adjustmentHandRot * _grabLocalRotation;
            var positiveDirection = worldRotation * Vector3.forward;
            _rotationPlaneTracker.Reset(worldRotation, positiveDirection);
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            var worldRotation = adjustmentHandRot * _grabLocalRotation;

            if (!_rotationPlaneTracker.Update(worldRotation)) return;

            var worldNormal = _rotationPlaneTracker.GetNormal();
            var localNormal = TransformUtils.WorldToLocalDirection(worldNormal, adjustmentHandRot);
            RecordNewMeasurement(localNormal);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = GetAverageMeasurement();
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = GetAverageMeasurement();
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
        ) { }

        #endregion
    }
}