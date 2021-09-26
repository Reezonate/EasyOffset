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
            4f,
            4f
        ) { }

        #endregion

        #region Logic

        private Vector3 _storedLocalDirection;
        private Vector3 _grabWorldDirection;
        private Vector3 _grabPosition;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            Vector3 grabPivotPosition;

            switch (adjustmentHand) {
                case Hand.Left:
                    grabPivotPosition = PluginConfig.LeftHandPivotPosition;
                    _storedLocalDirection = PluginConfig.LeftHandSaberDirection;
                    break;
                case Hand.Right:
                    grabPivotPosition = PluginConfig.RightHandPivotPosition;
                    _storedLocalDirection = PluginConfig.RightHandSaberDirection;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }

            _grabWorldDirection = adjustmentHandRot * _storedLocalDirection;
            _grabPosition = adjustmentHandPos + adjustmentHandRot * grabPivotPosition;
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            var finalLocalDirection = Quaternion.Inverse(adjustmentHandRot) * _grabWorldDirection;
            var newPivotPosition = Quaternion.Inverse(adjustmentHandRot) * (_grabPosition - adjustmentHandPos);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftHandSaberDirection = finalLocalDirection;
                    PluginConfig.LeftHandPivotPosition = newPivotPosition;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandSaberDirection = finalLocalDirection;
                    PluginConfig.RightHandPivotPosition = newPivotPosition;
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