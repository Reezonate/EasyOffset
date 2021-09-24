using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    [UsedImplicitly]
    public class RoomOffsetManager : AbstractOffsetManager {
        #region Constructor

        public RoomOffsetManager(
            MainSettingsModelSO mainSettingsModelSO
        ) : base(
            mainSettingsModelSO,
            AdjustmentMode.RoomOffset,
            false
        ) { }

        #endregion

        #region Logic

        private Vector3 _grabPosition;
        // private Vector3 _grabInverseForwardVector;

        private Vector3 _grabRoomCenter;
        // private float _grabRoomRotation;

        protected override void OnGrabStarted(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            _grabPosition = controllerPosition;
            // _grabInverseForwardVector = Quaternion.Inverse(grabRotation) * Vector3.forward;

            _grabRoomCenter = MainSettingsModel.roomCenter.value;
            // _grabRoomRotation = _mainSettingsModelSO.roomRotation.value;
        }

        protected override void OnGrabUpdated(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) {
            var positionChange = controllerPosition - _grabPosition;

            MainSettingsModel.roomCenter.value = _grabRoomCenter - positionChange;

            // var modifiedForwardVector = currentRotation * _grabInverseForwardVector;
            // var rotationChange = Mathf.Atan2(-modifiedForwardVector.x, modifiedForwardVector.z) * Mathf.Rad2Deg;
            // _mainSettingsModelSO.roomRotation.value = _grabRoomRotation + rotationChange;
        }

        protected override void OnGrabFinished(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation) { }

        #endregion
    }
}