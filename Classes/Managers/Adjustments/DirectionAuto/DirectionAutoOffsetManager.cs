using System;
using System.Collections.Generic;
using System.Linq;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class DirectionAutoOffsetManager : AbstractOffsetManager {
        #region Constructor

        private readonly RotationPlaneTracker _rotationPlaneTracker;

        public DirectionAutoOffsetManager(
            MainSettingsModelSO mainSettingsModel
        ) : base(
            mainSettingsModel,
            AdjustmentMode.DirectionAuto,
            true
        ) {
            _rotationPlaneTracker = new RotationPlaneTracker(BundleLoader.SphereMesh);
        }

        #endregion

        #region Measurements and averaging

        private const int MaxMeasurementsCount = 60;

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


        protected override void OnGrabStarted(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            _grabLocalRotation = hand switch {
                Hand.Left => PluginConfig.LeftHandRotation,
                Hand.Right => PluginConfig.RightHandRotation,
                _ => throw new ArgumentOutOfRangeException(nameof(hand), hand, null)
            };

            ResetMeasurements();
            RecordNewMeasurement(_grabLocalRotation * Vector3.forward);

            var worldRotation = controllerRotation * _grabLocalRotation;
            _rotationPlaneTracker.Reset(worldRotation);
        }

        protected override void OnGrabUpdated(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            var worldRotation = controllerRotation * _grabLocalRotation;

            if (!_rotationPlaneTracker.Update(worldRotation)) return;

            var worldNormal = _rotationPlaneTracker.GetNormal();
            var localNormal = TransformUtils.WorldToLocalDirection(worldNormal, controllerRotation);
            RecordNewMeasurement(localNormal);

            switch (hand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = GetAverageMeasurement();
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = GetAverageMeasurement();
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
        }

        protected override void OnGrabFinished(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) { }

        #endregion
    }
}