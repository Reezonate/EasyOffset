using System;
using EasyOffset.Configuration;
using UnityEngine;
using UnityEngine.XR;

namespace EasyOffset {
    public static class TransformUtils {
        #region AdjustControllerTransform

        internal static void AdjustControllerTransform(
            XRNode node,
            Transform transform
        ) {
            switch (node) {
                case XRNode.LeftHand:
                    AdjustLeftControllerTransform(transform);
                    break;
                case XRNode.RightHand:
                    AdjustRightControllerTransform(transform);
                    break;
                case XRNode.LeftEye: return;
                case XRNode.RightEye: return;
                case XRNode.CenterEye: return;
                case XRNode.Head: return;
                case XRNode.GameController: return;
                case XRNode.TrackingReference: return;
                case XRNode.HardwareTracker: return;
                default: throw new ArgumentOutOfRangeException(nameof(node), node, null);
            }
        }

        private static void AdjustLeftControllerTransform(
            Transform transform
        ) {
            if (PluginConfig.SmoothingEnabled) {
                var position = Abomination.LeftPosition;
                var rotation = Abomination.LeftRotation;
                ApplyRoomOffset(PluginConfig.MainSettingsModel, ref position, ref rotation);
                transform.position = position;
                transform.rotation = rotation;
            }

            transform.Translate(PluginConfig.LeftHandTranslation);
            transform.rotation *= PluginConfig.LeftHandRotation;
        }

        private static void AdjustRightControllerTransform(
            Transform transform
        ) {
            if (PluginConfig.SmoothingEnabled) {
                var position = Abomination.RightPosition;
                var rotation = Abomination.RightRotation;
                ApplyRoomOffset(PluginConfig.MainSettingsModel, ref position, ref rotation);
                transform.position = position;
                transform.rotation = rotation;
            }

            transform.Translate(PluginConfig.RightHandTranslation);
            transform.rotation *= PluginConfig.RightHandRotation;
        }

        #endregion

        #region GetSaberLocalTransform

        public static Vector3 GetSaberLocalPosition(
            Vector3 pivotPosition,
            Vector3 saberDirection,
            float zOffset
        ) {
            var rotationCausedOffset = saberDirection * zOffset;
            return pivotPosition + rotationCausedOffset;
        }

        public static Quaternion GetSaberLocalRotation(
            Vector3 saberDirection
        ) {
            return Quaternion.LookRotation(saberDirection, Vector3.up);
        }

        #endregion

        #region ApplyRoomOffsets

        public static void ApplyRoomOffset(MainSettingsModelSO mainSettingsModel, ref Vector3 position, ref Quaternion rotation) {
            var roomRotation = Quaternion.Euler(0, mainSettingsModel.roomRotation, 0);
            position = mainSettingsModel.roomCenter + roomRotation * position;
            rotation = roomRotation * rotation;
        }

        public static void ApplyRoomOffsetToVector(MainSettingsModelSO mainSettingsModel, ref Vector3 position) {
            var roomRotation = Quaternion.Euler(0, mainSettingsModel.roomRotation, 0);
            position = mainSettingsModel.roomCenter + roomRotation * position;
        }

        public static void ApplyRoomOffsetToDirection(MainSettingsModelSO mainSettingsModel, ref Vector3 direction) {
            var roomRotation = Quaternion.Euler(0, mainSettingsModel.roomRotation, 0);
            direction = roomRotation * direction;
        }

        #endregion

        #region TransformFuntions

        public static Vector3 LocalToWorldVector(Vector3 localPosition, Vector3 parentPosition, Quaternion parentRotation) {
            return parentPosition + parentRotation * localPosition;
        }

        public static Vector3 WorldToLocalVector(Vector3 worldPosition, Vector3 parentPosition, Quaternion parentRotation) {
            return Quaternion.Inverse(parentRotation) * (worldPosition - parentPosition);
        }

        public static Vector3 LocalToWorldDirection(Vector3 localDirection, Quaternion parentRotation) {
            return parentRotation * localDirection;
        }

        public static Vector3 WorldToLocalDirection(Vector3 worldDirection, Quaternion parentRotation) {
            return Quaternion.Inverse(parentRotation) * worldDirection;
        }

        #endregion

        #region Spherical

        public static Vector3 SphericalToOrthoDirection(Vector2 sphericalDirectionRadians) {
            var xzScale = Mathf.Cos(sphericalDirectionRadians.x);
            var x = Mathf.Sin(sphericalDirectionRadians.y) * xzScale;
            var z = Mathf.Cos(sphericalDirectionRadians.y) * xzScale;
            var y = -Mathf.Sin(sphericalDirectionRadians.x);
            return new Vector3(x, y, z);
        }

        public static Vector2 OrthoToSphericalDirection(Vector3 orthoDirection) {
            var yDir = new Vector2(orthoDirection.z, orthoDirection.x);
            var yAngleRad = Mathf.Atan2(yDir.y, yDir.x);
            var xAngleRad = -Mathf.Atan2(orthoDirection.y, yDir.magnitude);
            return new Vector2(xAngleRad, yAngleRad);
        }

        #endregion
    }
}