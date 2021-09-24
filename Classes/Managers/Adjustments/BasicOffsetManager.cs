using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class BasicOffsetManager : AbstractOffsetManager {
        #region Constructor

        public BasicOffsetManager(
            MainSettingsModelSO mainSettingsModel
        ) : base(
            mainSettingsModel,
            AdjustmentMode.Basic,
            true
        ) { }

        #endregion

        #region Logic

        private Vector3 _storedLocalDirection;
        private Vector3 _grabWorldDirection;
        private Vector3 _grabPosition;

        protected override void OnGrabStarted(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            Vector3 grabPivotPosition;

            switch (hand) {
                case Hand.Left:
                    grabPivotPosition = PluginConfig.LeftHandPivotPosition;
                    _storedLocalDirection = PluginConfig.LeftHandSaberDirection;
                    break;
                case Hand.Right:
                    grabPivotPosition = PluginConfig.RightHandPivotPosition;
                    _storedLocalDirection = PluginConfig.RightHandSaberDirection;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }

            _grabWorldDirection = controllerRotation * _storedLocalDirection;
            _grabPosition = controllerPosition + controllerRotation * grabPivotPosition;
        }

        protected override void OnGrabUpdated(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            var finalLocalDirection = Quaternion.Inverse(controllerRotation) * _grabWorldDirection;
            var newPivotPosition = Quaternion.Inverse(controllerRotation) * (_grabPosition - controllerPosition);

            switch (hand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = finalLocalDirection;
                    PluginConfig.LeftHandPivotPosition = newPivotPosition;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = finalLocalDirection;
                    PluginConfig.RightHandPivotPosition = newPivotPosition;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
        }

        protected override void OnGrabFinished(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) { }

        #endregion
    }
}