using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class RoomOffsetAdjustmentModeManager : AbstractAdjustmentModeManager {
        #region Constructor

        public RoomOffsetAdjustmentModeManager(
            MainSettingsModelSO mainSettingsModelSO
        ) : base(
            mainSettingsModelSO,
            AdjustmentMode.RoomOffset,
            6f,
            6f
        ) { }

        #endregion

        #region Logic

        private Vector3 _grabPosition;
        // private Vector3 _grabInverseForwardVector;

        private Vector3 _grabRoomCenter;
        // private float _grabRoomRotation;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            _grabPosition = adjustmentHandPos;
            // _grabInverseForwardVector = Quaternion.Inverse(grabRotation) * Vector3.forward;

            _grabRoomCenter = MainSettingsModel.roomCenter.value;
            // _grabRoomRotation = _mainSettingsModelSO.roomRotation.value;
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        ) {
            var positionChange = adjustmentHandPos - _grabPosition;

            MainSettingsModel.roomCenter.value = _grabRoomCenter - positionChange;

            // var modifiedForwardVector = currentRotation * _grabInverseForwardVector;
            // var rotationChange = Mathf.Atan2(-modifiedForwardVector.x, modifiedForwardVector.z) * Mathf.Rad2Deg;
            // _mainSettingsModelSO.roomRotation.value = _grabRoomRotation + rotationChange;
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