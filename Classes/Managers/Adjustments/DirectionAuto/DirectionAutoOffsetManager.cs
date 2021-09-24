using System;
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

        #region AdjustmentLogic

        private Quaternion _grabLocalRotation;

        protected override void OnGrabStarted(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            _grabLocalRotation = hand switch {
                Hand.Left => PluginConfig.LeftHandRotation,
                Hand.Right => PluginConfig.RightHandRotation,
                _ => throw new ArgumentOutOfRangeException(nameof(hand), hand, null)
            };

            var worldRotation = controllerRotation * _grabLocalRotation;
            _rotationPlaneTracker.Reset(worldRotation);
        }

        protected override void OnGrabUpdated(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            var worldRotation = controllerRotation * _grabLocalRotation;
            _rotationPlaneTracker.Update(worldRotation);

            var worldNormal = _rotationPlaneTracker.GetPlane().normal;
            var localNormal = TransformUtils.WorldToLocalDirection(worldNormal, controllerRotation);

            switch (hand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = localNormal;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = localNormal;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
        }

        protected override void OnGrabFinished(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) { }

        #endregion
    }
}