using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class DirectionOnlyOffsetManager : GrabTracker {
        #region Constructor

        private readonly DirectionGridGizmosManager _gizmosManager;

        public DirectionOnlyOffsetManager(
            DirectionGridGizmosManager gizmosManager,
            MainSettingsModelSO mainSettingsModel
        ) : base(
            mainSettingsModel,
            GripButtonAction.DirectionOnly,
            true
        ) {
            _gizmosManager = gizmosManager;
        }

        #endregion

        #region Logic

        private Vector3 _storedLocalDirection;
        private Vector3 _grabWorldDirection;

        protected override void OnGrabStarted(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            _storedLocalDirection = hand switch {
                Hand.Left => PluginConfig.LeftHandSaberDirection,
                Hand.Right => PluginConfig.RightHandSaberDirection,
                _ => throw new ArgumentOutOfRangeException(nameof(hand), hand, null)
            };

            _grabWorldDirection = controllerRotation * _storedLocalDirection;
            _gizmosManager.SetState(hand, true);
        }

        protected override void OnGrabUpdated(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            var finalLocalDirection = Quaternion.Inverse(controllerRotation) * _grabWorldDirection;

            switch (hand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = finalLocalDirection;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = finalLocalDirection;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }

            var gizmosDirection = controllerRotation * _storedLocalDirection;
            TransformUtils.ApplyRoomOffsetToDirection(MainSettingsModel, ref gizmosDirection);
            _gizmosManager.UpdateGizmos(hand, gizmosDirection);
        }

        protected override void OnGrabFinished(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            _gizmosManager.SetState(hand, false);
        }

        #endregion
    }
}