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
            3f,
            6f
        ) { }

        #endregion

        #region Logic

        private Vector3 _grabWorldPosition;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            var storedLocalPosition = adjustmentHand switch {
                Hand.Left => PluginConfig.LeftHandPivotPosition,
                Hand.Right => PluginConfig.RightHandPivotPosition,
                _ => throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null)
            };

            _grabWorldPosition = TransformUtils.LocalToWorldVector(storedLocalPosition, adjustmentHandPos, adjustmentHandRot);
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            var currentPivotLocalPosition = TransformUtils.WorldToLocalVector(_grabWorldPosition, adjustmentHandPos, adjustmentHandRot);

            switch (adjustmentHand) {
                case Hand.Left:
                    PluginConfig.LeftHandPivotPosition = currentPivotLocalPosition;
                    break;
                case Hand.Right:
                    PluginConfig.RightHandPivotPosition = currentPivotLocalPosition;
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