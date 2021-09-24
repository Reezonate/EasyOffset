using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class PivotOnlyOffsetManager : AbstractOffsetManager {
        #region Constructor

        public PivotOnlyOffsetManager(
            MainSettingsModelSO mainSettingsModelSO
        ) : base(
            mainSettingsModelSO,
            AdjustmentMode.PivotOnly,
            true
        ) { }

        #endregion

        #region Logic

        private Vector3 _grabWorldPosition;

        protected override void OnGrabStarted(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            var storedLocalPosition = hand switch {
                Hand.Left => PluginConfig.LeftHandPivotPosition,
                Hand.Right => PluginConfig.RightHandPivotPosition,
                _ => throw new ArgumentOutOfRangeException(nameof(hand), hand, null)
            };

            _grabWorldPosition = TransformUtils.LocalToWorldVector(storedLocalPosition, controllerPosition, controllerRotation);
        }

        protected override void OnGrabUpdated(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            var currentPivotLocalPosition = TransformUtils.WorldToLocalVector(_grabWorldPosition, controllerPosition, controllerRotation);

            switch (hand) {
                case Hand.Left:
                    PluginConfig.LeftHandPivotPosition = currentPivotLocalPosition;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandPivotPosition = currentPivotLocalPosition;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
        }

        protected override void OnGrabFinished(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) { }

        #endregion
    }
}