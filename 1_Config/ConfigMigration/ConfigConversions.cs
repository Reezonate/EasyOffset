using UnityEngine;

namespace EasyOffset {
    public static class ConfigConversions {
        #region Built-in offsets

        private static readonly Vector3 ValvePositionOffset = new(0.0f, 0.022f, -0.01f);
        private static readonly Vector3 ValveRotationOffset = new(-16.3f, 0.0f, 0.0f);

        private static readonly Vector3 DefaultPositionOffset = new(0.0f, -0.008f, 0.0f);
        private static readonly Vector3 DefaultRotationOffset = new(-4.3f, 0.0f, 0.0f);

        private static void ApplyBuiltInOffsets(
            bool isValveController,
            bool isVRModeOculus,
            ref Vector3 position,
            ref Vector3 rotation
        ) {
            if (isVRModeOculus) return;
            if (isValveController) {
                position += ValvePositionOffset;
                rotation += ValveRotationOffset;
            } else {
                position += DefaultPositionOffset;
                rotation += DefaultRotationOffset;
            }
        }

        private static void RemoveBuiltInOffsets(
            bool isValveController,
            bool isVRModeOculus,
            ref Vector3 position,
            ref Vector3 rotation
        ) {
            if (isVRModeOculus) return;
            if (isValveController) {
                position -= ValvePositionOffset;
                rotation -= ValveRotationOffset;
            } else {
                position -= DefaultPositionOffset;
                rotation -= DefaultRotationOffset;
            }
        }

        #endregion

        #region OneHandConversion

        private static void OneHandConversion(
            bool useBaseGameAdjustmentMode,
            bool isValveController,
            bool isVRModeOculus,
            float zOffset,
            Vector3 gripPosition,
            Vector3 gripRotation,
            out Vector3 pivotPosition,
            out Vector3 saberDirection
        ) {
            ApplyBuiltInOffsets(isValveController, isVRModeOculus, ref gripPosition, ref gripRotation);
            var rotation = Quaternion.Euler(gripRotation);
            saberDirection = rotation * Vector3.forward;

            if (useBaseGameAdjustmentMode) {
                pivotPosition = rotation * gripPosition - saberDirection * zOffset;
            } else {
                pivotPosition = gripPosition - saberDirection * zOffset;
            }
        }

        private static void OneHandInverseConversion(
            bool useBaseGameAdjustmentMode,
            bool isValveController,
            bool isVRModeOculus,
            Vector3 saberTranslation,
            Quaternion saberRotation,
            out Vector3 gripPosition,
            out Vector3 gripRotation
        ) {
            gripPosition = saberTranslation;
            gripRotation = saberRotation.eulerAngles;
            
            gripRotation = new Vector3(
                ClampRotation(gripRotation.x),
                ClampRotation(gripRotation.y),
                ClampRotation(gripRotation.z)
                );

            if (useBaseGameAdjustmentMode) {
                gripPosition = Quaternion.Inverse(saberRotation) * gripPosition;
            }

            RemoveBuiltInOffsets(isValveController, isVRModeOculus, ref gripPosition, ref gripRotation);
        }

        private static float ClampRotation(float value) {
            return value switch {
                > 180 => -360 + value,
                < -180 => 360 - value,
                _ => value
            };
        }

        #endregion

        #region FromBaseGame

        public static void FromBaseGame(
            bool isValveController,
            bool isVRModeOculus,
            float zOffset,
            Vector3 position,
            Vector3 rotation,
            out Vector3 leftPivotPosition,
            out Vector3 rightPivotPosition,
            out Vector3 leftSaberDirection,
            out Vector3 rightSaberDirection
        ) {
            OneHandConversion(
                true,
                isValveController,
                isVRModeOculus,
                zOffset,
                position,
                rotation,
                out rightPivotPosition,
                out rightSaberDirection
            );

            leftPivotPosition = new Vector3(
                -rightPivotPosition.x,
                rightPivotPosition.y,
                rightPivotPosition.z
            );

            leftSaberDirection = new Vector3(
                -rightSaberDirection.x,
                rightSaberDirection.y,
                rightSaberDirection.z
            );
        }

        #endregion

        #region FromTailor

        public static void FromTailor(
            bool useBaseGameAdjustmentMode,
            bool isValveController,
            bool isVRModeOculus,
            float leftZOffset,
            float rightZOffset,
            Vector3 gripLeftPosition,
            Vector3 gripRightPosition,
            Vector3 gripLeftRotation,
            Vector3 gripRightRotation,
            out Vector3 leftPivotPosition,
            out Vector3 rightPivotPosition,
            out Vector3 leftSaberDirection,
            out Vector3 rightSaberDirection
        ) {
            OneHandConversion(
                useBaseGameAdjustmentMode,
                isValveController,
                isVRModeOculus,
                leftZOffset,
                gripLeftPosition,
                gripLeftRotation,
                out leftPivotPosition,
                out leftSaberDirection
            );

            OneHandConversion(
                useBaseGameAdjustmentMode,
                isValveController,
                isVRModeOculus,
                rightZOffset,
                gripRightPosition,
                gripRightRotation,
                out rightPivotPosition,
                out rightSaberDirection
            );
        }

        #endregion

        #region ToBaseGame

        public static void ToBaseGame(
            bool isValveController,
            bool isVRModeOculus,
            Vector3 saberTranslation,
            Quaternion saberRotation,
            out Vector3 position,
            out Vector3 rotation
        ) {
            OneHandInverseConversion(
                true,
                isValveController,
                isVRModeOculus,
                saberTranslation,
                saberRotation,
                out position,
                out rotation
            );
        }

        #endregion

        #region ToTailor

        public static void ToTailor(
            bool useBaseGameAdjustmentMode,
            bool isValveController,
            bool isVRModeOculus,
            Vector3 leftSaberTranslation,
            Quaternion leftSaberRotation,
            Vector3 rightSaberTranslation,
            Quaternion rightSaberRotation,
            out Vector3 gripLeftPosition,
            out Vector3 gripRightPosition,
            out Vector3 gripLeftRotation,
            out Vector3 gripRightRotation
        ) {
            OneHandInverseConversion(
                useBaseGameAdjustmentMode,
                isValveController,
                isVRModeOculus,
                leftSaberTranslation,
                leftSaberRotation,
                out gripLeftPosition,
                out gripLeftRotation
            );

            OneHandInverseConversion(
                useBaseGameAdjustmentMode,
                isValveController,
                isVRModeOculus,
                rightSaberTranslation,
                rightSaberRotation,
                out gripRightPosition,
                out gripRightRotation
            );
        }

        #endregion
    }
}